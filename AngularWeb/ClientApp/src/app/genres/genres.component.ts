import { Component, OnInit } from '@angular/core';
import { GenreDto } from '../shared/genre-dto.model';
import { GenreService } from '../shared/genre.service';

@Component({
  selector: 'app-genres',
  templateUrl: './genres.component.html',
  styles: [
  ]
})
export class GenresComponent implements OnInit {

  constructor(public service: GenreService) { }

  ngOnInit(): void {
    setInterval(() => {
      this.service.refreshList();
    }, 200);
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
