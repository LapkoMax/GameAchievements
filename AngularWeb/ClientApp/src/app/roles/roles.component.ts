import { Component, OnInit } from '@angular/core';
import { DataSeedService } from '../shared/data-seed.service';
import { Role } from '../shared/role';
import { RolesService } from '../shared/roles.service';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styles: [
  ]
})
export class RolesComponent implements OnInit {

  constructor(public service: RolesService, public seedService: DataSeedService) { }

  toAddCount: number = 0;

  ngOnInit(): void {
    setInterval(() => {
      this.service.refreshList();
    }, 200);
  }

  seedData() {
    if (this.toAddCount != 0) {
      this.seedService.seedData("role", this.toAddCount).toPromise()
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

  async populateForm(selectedRecord: Role) {
    this.service.isCreate = false;
    this.service.errors = [];
    this.service.formData = Object.assign({}, selectedRecord);
  }

  onDelete(id: string) {
    this.service.deleteRole(id)
      .subscribe(
        res => {
          this.service.refreshList();
        },
        err => { console.log(err) }
      );
  }

}
