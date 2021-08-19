import { Component, OnInit } from '@angular/core';
import { DataSeedService } from '../shared/data-seed.service';
import { UserDto } from '../shared/user-dto.model';
import { UserForRegistrationDto } from '../shared/user-for-registration-dto.model';
import { UserService } from '../shared/user.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styles: [
  ]
})
export class UsersComponent implements OnInit {

  constructor(public service: UserService, public seedService: DataSeedService) { }

  toAddCount: number = 0;

  ngOnInit(): void {
    this.service.refreshList();
    setInterval(() => {
      this.service.refreshList();
    }, 20000);
  }

  seedData() {
    if (this.toAddCount != 0) {
      this.seedService.seedData("user", this.toAddCount).toPromise()
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

  async populateForm(selectedRecord: UserDto) {
    this.service.errors = [];
    this.service.formData = Object.assign({}, selectedRecord) as UserForRegistrationDto;
    this.service.userIdForUpdate = selectedRecord.id;
    this.service.userName = selectedRecord.userName;
    let roleIds = "";
    selectedRecord.roles.forEach(roleName => {
      this.service.roles.forEach(role => {
        if (roleName == role.name) roleIds += role.id + " ";
      });
    });
    this.service.rolesToAdd = roleIds;
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
