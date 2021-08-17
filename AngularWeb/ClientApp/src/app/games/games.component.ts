import { Component, OnInit } from '@angular/core';
import { DataSeedService } from '../shared/data-seed.service';
import { GameDto } from '../shared/game-dto.model';
import { GameDtoService } from '../shared/game-dto.service';

@Component({
  selector: 'app-games',
  templateUrl: './games.component.html',
  styles: [
  ]
})
export class GamesComponent implements OnInit {

  constructor(public service: GameDtoService, public seedService: DataSeedService) { }

  isEasterEgg: boolean = false;
  toAddCount: number = 0;

  ngOnInit(): void {
    setInterval(() => {
      this.service.refreshList();
    }, 200);
  }

  seedData() {
    if (this.toAddCount != 0) {
      this.seedService.seedData("game", this.toAddCount).toPromise()
        .then(
          res => {
            this.service.refreshList();
          },
          err => { console.log(err) }
        );
    }
  }

  changeToAddCount(event: any) {
    this.toAddCount = event.target.value;
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
