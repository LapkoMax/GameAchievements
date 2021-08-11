import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AchievementDto } from '../shared/achievement-dto.model';
import { AchievementService } from '../shared/achievement.service';

@Component({
  selector: 'app-achievements',
  templateUrl: './achievements.component.html',
  styles: [
  ]
})
export class AchievementsComponent implements OnInit {

  constructor(private route: ActivatedRoute, public service: AchievementService) { }

  gameId: number = 0;

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.gameId = +params['id'];
    });
    this.service.gameId = this.gameId;
    setInterval(() => {
      this.service.refreshList();
    }, 200);
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
