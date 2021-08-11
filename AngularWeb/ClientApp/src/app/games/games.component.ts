import { Component, OnInit } from '@angular/core';
import { GameDto } from '../shared/game-dto.model';
import { GameDtoService } from '../shared/game-dto.service';

@Component({
  selector: 'app-games',
  templateUrl: './games.component.html',
  styles: [
  ]
})
export class GamesComponent implements OnInit {

  constructor(public service: GameDtoService) { }

  isEasterEgg: boolean = false;

  ngOnInit(): void {
    setInterval(() => {
      this.service.refreshList();
    }, 200);
  }

  async populateForm(selectedRecord: GameDto) {
    this.service.formData = Object.assign({}, selectedRecord);
    let genres = await this.service.getGameGenres(selectedRecord.id);
    let genreIds = '';
    genres.forEach(genre => {
      genreIds += genre.id + " ";
    });
    this.service.genreIds = genreIds;
  }

  onDelete(id: number) {
    this.service.deleteGame(id)
      .subscribe(
        res => {
          this.service.refreshList();
        },
        err => { console.log(err) }
      );
  }

  easterEgg() {
    this.isEasterEgg = !this.isEasterEgg;
  }

}
