import { Component, OnInit } from '@angular/core';
import { GameDtoService } from '../../shared/game-dto.service';
import { GenreDto } from '../../shared/genre-dto.model';

@Component({
  selector: 'app-game-options-form',
  templateUrl: './game-options-form.component.html',
  styles: [
  ]
})
export class GameOptionsFormComponent implements OnInit {

  constructor(public service: GameDtoService) { }

  sortBy: string = "";
  byDescending: boolean = false;
  searchTerm: string = "";
  minRating: string = "0";
  maxRating: string = "10";
  fields: string = "name description rating genres";
  pageSize: number = 10;
  currentPage: number = 1;
  genreIds: string = "";
  genres: GenreDto[] = [];

  ngOnInit(): void {
  }

  sortByClick(event: any) {
    if (!event.target.value.includes("Choose")) this.sortBy = event.target.value;
  }

  descendingCheck(event: any) {
    this.byDescending = event.target.checked;
  }

  searchTermChange(event: any) {
    this.searchTerm = event.target.value;
  }

  minRatingChange(event: any) {
    this.minRating = event.target.value;
  }

  maxRatingChange(event: any) {
    this.maxRating = event.target.value;
  }

  fieldCheck(event: any) {
    if (event.target.checked && !this.fields.includes(event.target.value)) this.fields += " " + event.target.value;
    else if (!event.target.checked && this.fields.includes(event.target.value)) {
      let fields = this.fields.split(' ');
      this.fields = "";
      fields.forEach(field => {
        if (field != event.target.value) this.fields += field + " ";
      });
      this.fields = this.fields.trim();
    }
  }

  sortByGenreClick(event: any) {
    if (!event.target.value.includes("Choose")) {
      let ids = this.genreIds.split(' ');
      let isAdd = true;
      ids.forEach(genreId => {
        if (genreId == event.target.value) isAdd = false;
      });
      if (isAdd) {
        this.genreIds += event.target.value + " ";
        this.service.genresList.forEach(genre => { if (event.target.value == genre.id.toString()) this.genres.push(genre) });
      }
      this.genreIds.trim();
    }
  }

  onGenreDelete(id: number) {
    let ids = this.genreIds.trim().split(' ');
    this.genreIds = "";
    ids.forEach(genreId => {
      if (genreId != id.toString()) this.genreIds += genreId + " ";
    });
    this.genreIds.trim();
    let newGenres: GenreDto[] = [];
    this.genres.forEach(genre => {
      if (genre.id != id) newGenres.push(genre);
    });
    this.genres = newGenres;
  }

  pageOptionsChange(event: any) {
    this.pageSize = event.pageSize;
    this.currentPage = event.pageIndex;
    this.submitOptions();
    this.service.refreshList();
  }

  submitOptions() {
    let options = '';
    if (this.sortBy) options = "?orderBy=" + this.sortBy;
    else options = "?orderBy=name";
    if (this.byDescending) options += " desc";
    if (this.searchTerm) options += "&searchTerm=" + this.searchTerm;
    if (this.minRating) options += "&minRating=" + this.minRating;
    if (this.maxRating && parseFloat(this.minRating) < parseFloat(this.maxRating)) options += "&maxRating=" + this.maxRating;
    options += "&pageSize=" + this.pageSize + "&pageNumber=" + (+this.currentPage + 1);
    if (this.genreIds) options += "&genreIds=" + this.genreIds;
    this.service.options = options;
    this.service.fields = this.fields;
    this.service.refreshList();
  }

}
