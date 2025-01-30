import { Component, inject, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ReservationService } from '../../core/services/reservation.service';
import { CreateReservation } from '../../shared/models/createReservation';
import { Reservation } from '../../shared/models/reservation';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CommonModule } from '@angular/common';
import {
  MatDatetimepickerModule,
  MatNativeDatetimeModule,
} from '@mat-datetimepicker/core'; 

import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-add-reservation',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatFormFieldModule,
    MatDatetimepickerModule,
    MatNativeDatetimeModule,
    MatDatetimepickerModule,
    MatNativeDatetimeModule,
    MatDialogModule,
    CommonModule,
  ],
  templateUrl: './add-reservation.component.html',
  styleUrls: ['./add-reservation.component.css'],
})
export class AddReservationComponent implements OnInit {
  form = new FormGroup({
    id: new FormControl(''),
    name: new FormControl(''),
    lastName: new FormControl(''),
    fligthNumber: new FormControl(''),
    departureDate: new FormControl(new Date('2002-07-20 02-22')),
    landingDate: new FormControl(new Date('2002-07-20 02-22')),
    ticketClass: new FormControl(0),
  });
  constructor(
    private toastr: ToastrService,
    public dialogRef: MatDialogRef<AddReservationComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Reservation
  ) {}

  ngOnInit(): void {
    if (this.data != undefined) {
      this.form.patchValue({
        id: this.data.id,
        name: this.data.name || '',
        lastName: this.data.lastName || '',
        fligthNumber: this.data.fligthNumber || '',
        departureDate: this.data.departureDate || new Date('2002-07-20 02-22'),
        landingDate: this.data.landingDate || new Date('2002-07-20 02-22'),
        ticketClass: this.data.ticketClass || 0,
      });
    }
  }

  private reservationService = inject(ReservationService);

  onSubmit(): void {
    if (this.form.controls.id.value === '') {
      const createReservation: CreateReservation = {
        name: this.form.controls.name.value ?? '',
        lastName: this.form.controls.lastName.value ?? '',
        fligthNumber: this.form.controls.fligthNumber.value ?? '',
        departureDate:
          this.form.controls.departureDate.value ??
          new Date('2002-07-20 02-22'),
        landingDate:
          this.form.controls.landingDate.value ?? new Date('2002-07-20 02-22'),
        ticketClass: this.form.controls.ticketClass.value ?? 0,
      };
      this.reservationService.addReservation(createReservation).subscribe({
        next: (response) => {
          this.toastr.success('Success', 'New reservation added!');
          this.dialogRef.close(true);
          this.resetForm();
        },
        error: (err) =>
          this.toastr.error('Error', 'Erorr during adding reservation!'),
      });
    } else {
      const editReservation: Reservation = {
        id: this.form.controls.id.value ?? '',
        name: this.form.controls.name.value ?? '',
        lastName: this.form.controls.lastName.value ?? '',
        fligthNumber: this.form.controls.fligthNumber.value ?? '',
        departureDate:
          this.form.controls.departureDate.value ??
          new Date('2002-07-20 02-22'),
        landingDate:
          this.form.controls.landingDate.value ?? new Date('2002-07-20 02-22'),
        ticketClass: this.form.controls.ticketClass.value ?? 0,
      };
      this.reservationService.editReservation(editReservation).subscribe({
        next: (response) => {
          this.toastr.success('Success', 'Reservation modified!');
          this.dialogRef.close(true);
          this.resetForm();
        },
        error: (err) =>
          this.toastr.error('Error', 'Erorr during updating reservation!'),
      });
    }
  }

  resetForm(): void {
    this.form.reset({
      id: '',
      name: '',
      lastName: '',
      fligthNumber: '',
      departureDate: new Date('2002-07-20 02-22'),
      landingDate: new Date('2002-07-20 02-22'),
      ticketClass: 0,
    });
  }
}
