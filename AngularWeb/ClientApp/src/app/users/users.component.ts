import { Component, OnInit } from '@angular/core';
import { UserForRegistrationDto } from '../shared/user-for-registration-dto.model';
import { UserService } from '../shared/user.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styles: [
  ]
})
export class UsersComponent implements OnInit {

  constructor(public service: UserService) { }

  ngOnInit(): void {
    setInterval(() => {
      this.service.refreshList();
    }, 200);
  }

  async populateForm(selectedRecord: UserForRegistrationDto) {
    this.service.errors = [];
    this.service.formData = Object.assign({}, selectedRecord);
    this.service.userNameForUpdate = selectedRecord.userName;
    this.service.userName = selectedRecord.userName;
    let roleIds = "";
    selectedRecord.roles.forEach(roleName => {
      this.service.roles.forEach(role => {
        if (roleName == role.name) roleIds += role.id + " ";
      });
    });
    this.service.rolesToAdd = roleIds;
  }

  onDelete(userName: string) {
    this.service.deleteUser(userName)
      .subscribe(
        res => {
          this.service.refreshList();
        },
        err => { console.log(err) }
      );
  }

}
