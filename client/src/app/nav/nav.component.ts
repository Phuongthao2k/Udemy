import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Route, Router } from '@angular/router';
import { ToastrModule, ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  model: any = {};
  loggedIn = false;

  constructor(public accountService: AccountService, private router: Router,
    private toastr: ToastrService) {}
  ngOnInit(): void {}

  getCurrentUser() {
    this.accountService.currentUser$.subscribe({
      next: _ => this.router.navigateByUrl('/members'),
      error: error => this.toastr.error(error.error)
    });
  }

  login() {
    this.accountService.login(this.model).subscribe({
      next: (_) => this.router.navigateByUrl('/members'),
      error: (error) => console.log(error),
    });
  }

  logout() {
    this.accountService.logout();
    this.loggedIn = false;
  }
}
