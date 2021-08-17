import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { UserDto } from '../../shared/user-dto.model';
import { UserForRegistrationDto } from '../../shared/user-for-registration-dto.model';
import { UserService } from '../../shared/user.service';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styles: [
  ]
})
export class UserFormComponent implements OnInit {

  constructor(public service: UserService) { }

  ngOnInit(): void {
  }

  onSubmit(form: NgForm) {
    this.service.errors = [];
    if (this.service.formData.password) {
      this.insertRecord(form);
    }
    else {
      this.updateRecord(form);
    }
  }

  insertRecord(form: NgForm) {
    this.service.postUser().subscribe(
      res => {
        this.resetForm(form);
        this.service.refreshList();
      },
      err => {
        this.service.errors = JSON.stringify(err.error).replace(/{|}|\[|\]|"/g, '').split(",");
      }
    );
    if (this.service.rolesToAdd != "") {
      this.service.updateRolesForUser().subscribe(
        res => { },
        err => { console.log(err) }
      );
    }
  }

  updateRecord(form: NgForm) {
    this.service.putUser().subscribe(
      res => {
        if (res == "") {
          this.resetForm(form);
          this.service.refreshList();
        }
        else {
          this.service.errors = JSON.stringify(res).replace(/{|}|\[|\]|code|description|:|\"/g, '').split(',');
        }
      },
      err => {
        this.service.errors = JSON.stringify(err.error.errors).replace(/{|}|\[|\]|"/g, '').split(',');
      }
    );
    if (this.service.rolesToAdd != "") {
      this.service.updateRolesForUser().subscribe(
        res => { },
        err => { console.log(err.errors) }
      );
    }
  }

  onRoleSelect(event: any) {
    if (!event.target.value.includes("Choose") && !this.service.rolesToAdd.includes(event.target.value)) this.service.rolesToAdd += event.target.value + " ";
  }

  resetForm(form: NgForm) {
    this.service.userIdForUpdate = "";
    this.service.rolesToAdd = "";
    form.form.reset();
    this.service.formData = new UserForRegistrationDto();
  }

}
