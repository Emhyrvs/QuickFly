import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ReservationsComponent } from './features/reservations/reservations.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: true,
  styleUrl: './app.component.css',
  imports: [ReservationsComponent],
})
export class AppComponent {
  constructor(private http: HttpClient) {}
}
