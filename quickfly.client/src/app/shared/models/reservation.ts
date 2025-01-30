import { DatetimeAdapter } from '@mat-datetimepicker/core';

export interface Reservation {
  id: string;
  name: string;
  lastName: string;
  fligthNumber: string;
  departureDate: Date;
  landingDate: Date;
  ticketClass: number;
}
