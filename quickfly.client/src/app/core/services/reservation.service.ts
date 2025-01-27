import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Reservation } from '../../shared/models/reservation';
import { environment } from '../../../environments/environments';
import { CreateReservation } from '../../shared/models/createReservation';
import { SortDirection } from '@angular/material/sort';

@Injectable({
  providedIn: 'root',
})
export class ReservationService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getReservations(
    page: number,
    size: number,
    filter: string,
    active: string,
    direction: SortDirection
  ): Observable<{ data: Reservation[]; totalSize: number }> {
    return this.http.get<{ data: Reservation[]; totalSize: number }>(
      `${
        this.baseUrl
      }reservations?PageNumber=${page}&PageSize=${size}&Filter=${filter}&Active=${active}&Direction=${direction.toString()}`
    );
  }

  addReservation(reservation: CreateReservation): Observable<any> {
    return this.http.post<Reservation>(
      `${this.baseUrl}reservations`,
      reservation
    );
  }
  deleteReservation(id: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}reservations/${id}`);
  }
  editReservation(reservation: Reservation): Observable<any> {
    return this.http.put(`${this.baseUrl}reservations`, reservation);
  }
}
