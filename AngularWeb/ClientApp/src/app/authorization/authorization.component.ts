import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorizationService } from '../shared/authorization.service';

@Component({
  selector: 'app-authorization',
  templateUrl: './authorization.component.html',
  styles: [
  ]
})
export class AuthorizationComponent implements OnInit {

  constructor(public service: AuthorizationService, private router: Router) { }

  isRegister: boolean = false;

  ngOnInit(): void {
  }

  registerClick(event: any) {
    if (!this.isRegister) this.isRegister = true;
    else if (this.isRegister && this.service.status != "400" && this.service.status != "401") {
      this.service.registerUser();
      this.service.status = "200";
      this.isRegister = false
    }
  }

  onSubmit() {
    this.service.authenticate();
    if (this.service.status != "400" && this.service.status != "401") this.router.navigate([""]);
  }

}
