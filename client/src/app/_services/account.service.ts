import { HttpClient } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AccountService implements OnInit {
  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();
  constructor(private http: HttpClient) {}
  // user: User = {
  //   username: "lisa",
  //   token: ''
  // };

  ngOnInit(): void {
    //this.currentUserSource.next(this.user);
  }

  // login(model: any) {
  //   return this.http.post(this.baseUrl + 'account/login', model).pipe(
  //     map((response: User) => {
  //       const user = response;
  //       if (user) {
  //         this.setCurrentUser(user);
  //         this.presence.createHubConnection(user);
  //       }
  //     })
  //   )
  // }

  // setCurrentUser(user: User): void {
  //   user.roles = [];
  //   const roles = this.getDecodedToken(user.token).role;
  //   Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
  //   localStorage.setItem('user', JSON.stringify(user));
  //   this.currentUserSource.next(user);
  // }

  // getDecodedToken(token: string) {
  //   return JSON.parse(atob(token.split('.')[1]));
  // }

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    );
  }

  register(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map((user) => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
        return user;
      })
    );
  }

  setCurrentUser(user: User) {
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
  }
}
