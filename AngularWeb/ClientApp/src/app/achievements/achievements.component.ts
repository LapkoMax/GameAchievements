import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AchievementDto } from '../shared/achievement-dto.model';
import { AchievementService } from '../shared/achievement.service';
import { DataSeedService } from '../shared/data-seed.service';

@Component({
  selector: 'app-achievements',
  templateUrl: './achievements.component.html',
  styles: [
  ]
})
export class AchievementsComponent implements OnInit {

  constructor(private route: ActivatedRoute, public service: AchievementService, public seedService: DataSeedService) { }

  gameId: number = 0;
  toAddCount: number = 0;

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.gameId = +params['id'];
    });
    this.service.gameId = this.gameId;
    this.service.refreshList();
    setInterval(() => {
      this.service.refreshList();
    }, 20000);
  }

  seedData() {
    if (this.toAddCount != 0) {
      this.seedService.seedData("achievement", this.toAddCount, this.gameId).toPromise()
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

  async populateForm(selectedRecord: AchievementDto) {
    this.service.formData = Object.assign({}, selectedRecord);
  }

  onDelete(id: number) {
    this.service.deleteAchievement(id)
      .subscribe(
        res => {
          this.service.refreshList();
        },
        err => { console.log(err) }
      );
  }

}
