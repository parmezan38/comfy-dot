import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { NavbarComponent } from './navbar/navbar.component';
import { LoginComponent } from './login/login.component';
import { ChatComponent } from './chat/chat.component';
import { ChatMessageComponent } from './chat/chat-message.component';
import { CallbackComponent } from './callback/callback.component';
import { LobbyComponent } from './chat/lobby.component';
import { RoomComponent } from './chat/room.component';
import { NewRoomComponent } from './chat/new-room.component';

import { AuthService } from './services/auth.service';
import { ApiService } from './services/api.service';
import { SignalRService } from './services/signalR.service';
import { AuthGuard } from './services/auth.guard';

import { NgxsModule } from '@ngxs/store';
import { NgxsStoragePluginModule } from '@ngxs/storage-plugin';
import { LobbyState } from './state/lobby.state';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    LoginComponent,
    ChatComponent,
    ChatMessageComponent,
    CallbackComponent,
    LobbyComponent,
    RoomComponent,
    NewRoomComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    NgxsModule.forRoot([ LobbyState ]),
    NgxsStoragePluginModule.forRoot()
  ],
  providers: [
    AuthService,
    ApiService,
    AuthGuard,
    NgxsModule,
    SignalRService,
    { provide: 'Window', useValue: window }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
