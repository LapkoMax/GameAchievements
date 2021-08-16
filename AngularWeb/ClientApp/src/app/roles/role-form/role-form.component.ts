import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Role } from '../../shared/role';
import { RolesService } from '../../shared/roles.service';

@Component({
  selector: 'app-role-form',
  templateUrl: './role-form.component.html',
  styles: [
  ]
})
export class RoleFormComponent implements OnInit {

  constructor(public service: RolesService) { }

  ngOnInit(): void {
  }

  onSubmit(form: NgForm) {
    this.service.errors = [];
    if (this.service.isCreate) {
      this.insertRecord(form);
    }
    else {
      this.updateRecord(form);
    }
  }

  insertRecord(form: NgForm) {
    this.service.postRole().subscribe(
      res => {
        this.resetForm(form);
        this.service.refreshList();
      },
      err => {
        console.log(err);
        this.service.errors = JSON.stringify(err.error.errors).replace(/{|}|\[|\]|"/g, '').split(",");
      }
    );
  }

  updateRecord(form: NgForm) {
    this.service.putRole().subscribe(
      res => {
        this.resetForm(form);
        this.service.refreshList();
        this.service.isCreate = true;
      },
      err => {
        this.service.errors = JSON.stringify(err.error.errors).replace(/{|}|\[|\]|"/g, '').split(',');
      }
    );
  }

  resetForm(form: NgForm) {
    form.form.reset();
    this.service.formData = new Role();
  }

}
