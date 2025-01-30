import { Reservation } from './reservation';

export interface PagedReservation {
  reservations: Reservation[];
  totalCount: number;
}
