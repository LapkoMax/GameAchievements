import { Component, OnInit } from '@angular/core';
import { DataSeedService } from '../shared/data-seed.service';
import { GenreDto } from '../shared/genre-dto.model';
import { GenreService } from '../shared/genre.service';

@Component({
  selector: 'app-genres',
  templateUrl: './genres.component.html',
  styles: [
  ]
})
export class GenresComponent implements OnInit {

  constructor(public service: GenreService, public seedService: DataSeedService) { }

  toAddCount: number = 0;

  ngOnInit(): void {
    this.service.refreshList();
    setInterval(() => {
      this.service.refreshList();
    }, 20000);
  }

  seedData() {
    if (this.toAddCount != 0) {
      this.seedService.seedData("genre", this.toAddCount).toPromise()
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

  async populateForm(selectedRecord: GenreDto) {
    this.service.formData = Object.assign({}, selectedRecord);
  }

  onDelete(id: number) {
    this.service.deleteGenre(id)
      .subscribe(
        res => {
          this.service.refreshList();
        },
        err => { console.log(err) }
      );
  }

}
