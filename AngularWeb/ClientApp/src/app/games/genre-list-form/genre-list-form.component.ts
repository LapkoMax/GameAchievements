import { Component, Input, OnInit } from '@angular/core';
import { GameDtoService } from '../../shared/game-dto.service';
import { GenreDto } from '../../shared/genre-dto.model';

@Component({
  selector: 'app-genre-list-form',
  templateUrl: './genre-list-form.component.html',
  styles: [
  ]
})
export class GenreListFormComponent implements OnInit {

  constructor(public service: GameDtoService) { }

  @Input() public genreIds: string = '';
  @Input() public genreDtos: GenreDto[] = [];
  genres: GenreDto[] = [];

  ngOnChanges(changes: any) {
    this.ngOnInit();
    if (this.genreIds == '') this.genres = [];
  }

  onGenreRemove(id: number) {
    let newGenreIds = '';
    let ids = this.genreIds.split(' ');
    ids.forEach(genreId => {
      if (genreId != id.toString()) newGenreIds += genreId + ' ';
    });
    this.service.genreIds = newGenreIds + ' ';
  }

  ngOnInit(): void {
    this.genres = [];
    let ids = this.genreIds.split(' ');
    ids.forEach(id => {
      this.genreDtos.forEach(genre => {
        if (genre.id.toString() == id) this.genres.push(genre);
      });
    });
  }

}
