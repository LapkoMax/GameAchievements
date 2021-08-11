import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { GenreDto } from '../../shared/genre-dto.model';
import { GenreService } from '../../shared/genre.service';

@Component({
  selector: 'app-genre-form',
  templateUrl: './genre-form.component.html',
  styles: [
  ]
})
export class GenreFormComponent implements OnInit {

  constructor(public service: GenreService) { }

  ngOnInit(): void {
  }

  onSubmit(form: NgForm) {
    if (this.service.formData.id == 0) {
      this.insertRecord(form);
    }
    else {
      this.updateRecord(form);
    }
  }

  insertRecord(form: NgForm) {
    this.service.postGenre().subscribe(
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
    this.service.putGenre().subscribe(
      res => {
        this.resetForm(form);
        this.service.refreshList();
      },
      err => {
        console.log(err);
      }
    );
  }

  resetForm(form: NgForm) {
    form.form.reset();
    this.service.formData = new GenreDto();
  }

}
