import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { ChatComponent } from './chat/chat.component';
import { CallbackComponent } from './callback/callback.component';
import { SilentComponent } from './callback/silent.component';
import { LobbyComponent } from './chat/lobby.component';
import { AuthGuard } from './services/auth.guard';

const routes: Routes = [
  {
    path: '',
    component: LobbyComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard]
  },
  {
    path: 'login',
    component: LoginComponent,
    pathMatch: 'full'
  },
  {
    path: 'room/:id',
    component: ChatComponent
  },
  {
    path: 'callback',
    component: CallbackComponent,
    pathMatch: 'full'
  },
  {
    path: 'silent',
    component: SilentComponent,
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
