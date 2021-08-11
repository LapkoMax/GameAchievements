import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AchievementDto } from '../../shared/achievement-dto.model';
import { AchievementService } from '../../shared/achievement.service';

@Component({
  selector: 'app-achievement-form',
  templateUrl: './achievement-form.component.html',
  styles: [
  ]
})
export class AchievementFormComponent implements OnInit {

  constructor(public service: AchievementService) { }

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
    this.service.postAchievement().subscribe(
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
    this.service.putAchievement().subscribe(
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
    this.service.formData = new AchievementDto();
  }

}
