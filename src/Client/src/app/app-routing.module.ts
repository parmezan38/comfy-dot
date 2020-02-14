import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { ChatComponent } from './chat/chat.component';
import { CallbackComponent } from './callback/callback.component';
import { LobbyComponent } from './chat/lobby.component';
import { RoomComponent } from './chat/room.component';
import { NewRoomComponent } from './chat/new-room.component';
import { AuthGuard } from './shared/auth.guard';

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
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
