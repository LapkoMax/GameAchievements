import { Component, OnInit } from '@angular/core';
import { GameDtoService } from '../../shared/game-dto.service';
import { NgForm } from '@angular/forms';
import { GameDto } from '../../shared/game-dto.model';
import { GenreDto } from '../../shared/genre-dto.model';

@Component({
  selector: 'app-game-form',
  templateUrl: './game-form.component.html',
  styles: [
  ]
})
export class GameFormComponent implements OnInit {

  constructor(public service: GameDtoService) { }

  ngOnInit(): void {
    this.service.http.get(this.service._baseGenreUrl + "?pageSize=2147483647")
      .toPromise()
      .then(res => this.service.genresList = res as GenreDto[]);
    setInterval(() => {
      this.checkServiceGenreIds();
    }, 200);
  }

  ngOnDestroy() {
    clearInterval();
  }

  genreIds: string = '';

  checkServiceGenreIds() {
    if (this.service.genreIds != '') this.genreIds = this.service.genreIds.trim();
  }

  onSubmit(form: NgForm) {
    if (this.service.formData.id == 0) {
      this.insertRecord(form);
      this.updateGenresRecord(this.genreIds, form);
    }
    else {
      this.updateRecord(form);
      this.updateGenresRecord(this.genreIds, form);
    }
    this.service.genreIds = '';
    this.genreIds = '';
  }

  onGenreOptionClick(event: any) {
    if (!event.target.value.includes("Choose") && !this.genreIds.includes(event.target.value)) {
      this.service.genreIds += event.target.value + ' ';
    }
  }

  insertRecord(form: NgForm) {
    this.service.postGame().subscribe(
      res => {
        this.resetForm(form);
        this.service.refreshList();
      },
      err => {
        console.log(err);
      }
    );
  }

  updateRecord(form: NgForm) {
    this.service.putGame().subscribe(
      res => {
        this.resetForm(form);
        this.service.refreshList();
      },
      err => {
        console.log(err);
      }
    );
  }

  updateGenresRecord(genreIds: string, form: NgForm) {
    this.service.updateGenres(genreIds).subscribe(
      res => {
        this.resetForm(form);
        this.service.refreshList();
      },
      err => {
        console.log(err);
      });
  }

  resetForm(form: NgForm) {
    form.form.reset();
    this.service.formData = new GameDto();
  }

}
