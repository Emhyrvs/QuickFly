import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { ReservationService } from '../../core/services/reservation.service';
import { Reservation } from '../../shared/models/reservation';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { faPen, faTrash } from '@fortawesome/free-solid-svg-icons';

import { AddReservationComponent } from '../add-reservation/add-reservation.component';
import {
  MatPaginator,
  MatPaginatorModule,
  PageEvent,
} from '@angular/material/paginator';
import { MatSortModule, Sort, SortDirection } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { TicketClass } from '../../shared/ticketClass';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';

@Component({
  selector: 'app-reservations',
  templateUrl: './reservations.component.html',
  styleUrls: ['./reservations.component.css'],
  standalone: true,
  imports: [
    FontAwesomeModule,
    AddReservationComponent,
    MatSortModule,
    MatPaginatorModule,
    MatTableModule,
    MatButtonModule,
    MatDividerModule,
    MatIconModule,
  ],
})
export class ReservationsComponent implements OnInit {
  private reservationService = inject(ReservationService);
  reservations: Reservation[] = [];
  displayedColumns: string[] = [
    'id',
    'name',
    'lastName',
    'fligthNumber',
    'departureDate',
    'landingDate',
    'ticketClass',
    'actions',
  ];
  selectedReservation: Reservation | null = null;

  filterValue: string = '';

  constructor(library: FaIconLibrary) {
    library.addIcons(faTrash, faPen);
  }
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  totalSize = 100;
  pageSize = 10;
  pageIndex = 0;
  displayForm: boolean | null = false;

  onPageChange(event: PageEvent) {
    this.pageIndex = event.pageIndex ?? 0;
    this.pageSize = event.pageSize ?? 10;

    this.loadReservations(this.pageIndex, this.pageSize, '');
  }

  ngOnInit(): void {
    this.loadReservations(0, 10, this.filterValue); // Pobranie danych przy starcie (strona 0, 10 elementów na stronę)
  }
  edit(reservation: Reservation): void {
    this.displayForm = true;
    this.selectedReservation = reservation;
  }
  CloseForm() {
    this.displayForm = false;
  }
  onDisplayForm() {
    this.displayForm = true;
  }
  delete(id: string): void {
    this.reservationService.deleteReservation(id).subscribe({
      next: () => this.loadReservations(this.pageIndex, this.pageSize), // Odświeżenie rezerwacji po usunięciu
      error: (err) => {
        console.error('Error deleting reservation:', err);
        this.reservations = []; // Obsługa błędów
      },
    });
  }
  applyFilter(event: Event) {
    this.filterValue = (event.target as HTMLInputElement).value.toLowerCase();
    this.pageIndex = 0;
    this.loadReservations(this.pageIndex, this.pageSize, this.filterValue);
  }
  sortData(sort: Sort) {
    this.loadReservations(
      this.pageIndex,
      this.pageSize,
      this.filterValue,
      sort.active,
      sort.direction
    );
  }
  getTicketClassName(ticketClass: number): string {
    return TicketClass[ticketClass];
  }
  loadReservations(
    pageIndex: number = 0,
    pageSize: number = 10,
    filter: string = '',
    active: string = '',
    direction: SortDirection = ''
  ): void {
    this.reservationService
      .getReservations(pageIndex + 1, pageSize, filter, active, direction)
      .subscribe({
        next: (result: any) => {
          this.reservations = result.reservations;
          this.totalSize = result.totalSize;
        },
        error: (err) => {
          this.reservations = [];
        },
      });
  }
}
