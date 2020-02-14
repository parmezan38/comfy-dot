import { UserManager, User } from 'oidc-client';
import { Injectable, EventEmitter } from '@angular/core';
import { Observable, from } from 'rxjs';
import { IUser } from './interfaces';

const config: any = {
  // TODO: Change to https when changed on server
  client_id: 'js',
  response_type: 'code',
  scope: 'openid profile colors comfyDotApi',
  authority: 'http://localhost:5000',
  redirect_uri: 'http://localhost:4200/callback',
  post_logout_redirect_uri: 'http://localhost:4200',
  silent_redirect_uri: 'http://localhost:4200/silent',
  automaticSilentRenew: true
}

@Injectable()
export class AuthService {
  protected headers: Headers;
  protected loggedIn = false;
  protected user: User;
  public userProfile: IUser;
  public manager: UserManager = new UserManager(config);

  public userChangeEvent: EventEmitter<User> = new EventEmitter<User>();

  constructor() {
    this.manager.events.addUserLoaded(user => {
      this.user = user;
      this.loggedIn = true;
      this.userChangeEvent.emit(user);
    });
    this.manager.events.addUserUnloaded(() => {
      this.user = null;
      this.loggedIn = false;
      this.userChangeEvent.emit(null);
    });

    this.getUser();
  }

  private getUser() {
    return this.manager.getUser()
      .then((user) => {
        this.setUserAndLoggedIn(user);
        return !!user;
      })
      .catch((err) => {
        this.user = null;
        this.loggedIn = false;
      });
  }

  public getUserProfile(): Promise<any> {
    return this.manager.getUser()
      .then(user => {
        this.setUserAndLoggedIn(user);
        const profile: IUser = {
          id: user.profile.sub,
          name: user.profile.name,
          color1: user.profile.color1,
          color2: user.profile.color2
        }
        return profile;
      })
  }

  public getAccessToken(): string {
    let token: string = (this.user) ? this.user.access_token : null;
    return token;
  }

  public login(): void {
    this.manager
      .signinRedirect()
      .catch(error => {
        console.error('AuthService - Error while signing in:', error);
      });
  }

  public logout(): void {
    this.manager
      .signoutRedirect()
      .catch(error => {
        console.error('AuthService - Error while signing out:', error);
      });
  }

  public isLoggedIn(): Observable<boolean | void> {
    return from(this.getUser());
  }

  private setUserAndLoggedIn(user) {
    if (user) {
      this.user = user;
      this.loggedIn = true;
      this.userChangeEvent.emit(user);
    } else {
      this.user = null;
      this.loggedIn = false;
    }
  }

  public getUserId(): string {
    return this.user.profile.sub;
  }
}
