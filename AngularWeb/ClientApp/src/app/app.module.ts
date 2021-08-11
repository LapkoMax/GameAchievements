import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { GamesComponent } from './games/games.component';
import { GameFormComponent } from './games/game-form/game-form.component';
import { AchievementsComponent } from './achievements/achievements.component';

import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { GenreListFormComponent } from './games/genre-list-form/genre-list-form.component';
import { GameOptionsFormComponent } from './games/game-options-form/game-options-form.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MatPaginatorModule } from '@angular/material/paginator';
import { AchievementFormComponent } from './achievements/achievement-form/achievement-form.component';
import { AchievementOptionsFormComponent } from './achievements/achievement-options-form/achievement-options-form.component';
import { GenresComponent } from './genres/genres.component';
import { GenreFormComponent } from './genres/genre-form/genre-form.component';
import { GenreOptionsFormComponent } from './genres/genre-options-form/genre-options-form.component';

@NgModule({
  declarations: [
    AppComponent,
    GamesComponent,
    GameFormComponent,
    GenreListFormComponent,
    GameOptionsFormComponent,
    AchievementsComponent,
    AchievementFormComponent,
    AchievementOptionsFormComponent,
    GenresComponent,
    GenreFormComponent,
    GenreOptionsFormComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot([
      { path: '', component: GamesComponent },
      { path: 'achievements/:id', component: AchievementsComponent },
      { path: 'genres', component: GenresComponent }
    ]),
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatPaginatorModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
