import { Component, OnInit } from '@angular/core';
import { Role } from '../shared/role';
import { RolesService } from '../shared/roles.service';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styles: [
  ]
})
export class RolesComponent implements OnInit {

  constructor(public service: RolesService) { }

  ngOnInit(): void {
    setInterval(() => {
      this.service.refreshList();
    }, 200);
  }

  async populateForm(selectedRecord: Role) {
    this.service.isCreate = false;
    this.service.errors = [];
    this.service.formData = Object.assign({}, selectedRecord);
  }

  onDelete(id: string) {
    this.service.deleteUser(id)
      .subscribe(
        res => {
          this.service.refreshList();
        },
        err => { console.log(err) }
      );
  }

}
