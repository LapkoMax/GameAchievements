import { Component, OnInit } from '@angular/core';
import { AchievementService } from '../../shared/achievement.service';

@Component({
  selector: 'app-achievement-options-form',
  templateUrl: './achievement-options-form.component.html',
  styles: [
  ]
})
export class AchievementOptionsFormComponent implements OnInit {

  constructor(public service: AchievementService) { }

  sortBy: string = "";
  byDescending: boolean = false;
  searchTerm: string = "";
  fields: string = "name description condition";
  pageSize: number = 10;
  currentPage: number = 1;

  ngOnInit(): void {
  }

  sortByClick(event: any) {
    if (!event.target.value.includes("Choose")) this.sortBy = event.target.value;
  }

  descendingCheck(event: any) {
    this.byDescending = event.target.checked;
  }

  searchTermChange(event: any) {
    this.searchTerm = event.target.value;
  }

  fieldCheck(event: any) {
    if (event.target.checked && !this.fields.includes(event.target.value)) this.fields += " " + event.target.value;
    else if (!event.target.checked && this.fields.includes(event.target.value)) {
      let fields = this.fields.split(' ');
      this.fields = "";
      fields.forEach(field => {
        if (field != event.target.value) this.fields += field + " ";
      });
      this.fields = this.fields.trim();
    }
  }

  pageOptionsChange(event: any) {
    this.pageSize = event.pageSize;
    this.currentPage = event.pageIndex;
    this.submitOptions();
  }

  submitOptions() {
    let options = '';
    if (this.sortBy) options = "?orderBy=" + this.sortBy;
    else options = "?orderBy=name";
    if (this.byDescending) options += " desc";
    if (this.searchTerm) options += "&searchTerm=" + this.searchTerm;
    options += "&pageSize=" + this.pageSize + "&pageNumber=" + (+this.currentPage + 1);
    this.service.options = options;
    this.service.fields = this.fields;
  }

}
