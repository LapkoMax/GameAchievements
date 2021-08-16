import { Component, Input, OnInit } from '@angular/core';
import { Role } from '../../shared/role';
import { UserService } from '../../shared/user.service';

@Component({
  selector: 'app-role-list-form',
  templateUrl: './role-list-form.component.html',
  styles: [
  ]
})
export class RoleListFormComponent implements OnInit {

  constructor(public service: UserService) { }

  @Input() public roleIds: string = '';

  roles: Role[] = [];

  ngOnChanges(changes: any) {
    this.ngOnInit();
    if (this.roleIds == '') this.roles = [];
  }

  onRoleRemove(id: string) {
    let newRoleIds = '';
    let ids = this.roleIds.split(' ');
    ids.forEach(roleId => {
      if (roleId != id.toString()) newRoleIds += roleId + ' ';
    });
    this.service.rolesToAdd = newRoleIds;
  }

  ngOnInit(): void {
    this.roles = [];
    let ids = this.roleIds.split(' ');
    ids.forEach(id => {
      this.service.roles.forEach(role => {
        if (role.id == id) this.roles.push(role);
      });
    });
  }

}
