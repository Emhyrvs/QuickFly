import {
  Component,
  EventEmitter,
  inject,
  Input,
  OnChanges,
  Output,
  signal,
  SimpleChanges,
} from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ReservationService } from '../../core/services/reservation.service';
import { CreateReservation } from '../../shared/models/createReservation';
import { Reservation } from '../../shared/models/reservation';

@Component({
  selector: 'app-add-reservation',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './add-reservation.component.html',
  styleUrls: ['./add-reservation.component.css'],
})
export class AddReservationComponent implements OnChanges {
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['selectedReservation'] && this.selectedReservation) {
      this.form.patchValue({
        id: this.selectedReservation.id,
        name: this.selectedReservation.name || '',
        lastName: this.selectedReservation.lastName || '',
        fligthNumber: this.selectedReservation.fligthNumber || 1,
        departureDate: this.selectedReservation.departureDate || '',
        landingDate: this.selectedReservation.landingDate || '',
        ticketClass: this.selectedReservation.ticketClass || 0,
      });
    }
  }
  private reservationService = inject(ReservationService);
  @Output() onAddReservation = new EventEmitter<void>();
  @Output() onCloseForm = new EventEmitter<void>();
  @Input() selectedReservation: Reservation | null = null;
  @Input() displayForm: boolean | null = false;

  form = new FormGroup({
    id: new FormControl(''),
    name: new FormControl(''),
    lastName: new FormControl(''),
    fligthNumber: new FormControl(0),
    departureDate: new FormControl(''),
    landingDate: new FormControl(''),
    ticketClass: new FormControl(0),
  });

  onSubmit(): void {
    if (this.form.controls.id.value === '') {
      const createReservation: CreateReservation = {
        name: this.form.controls.name.value ?? '',
        lastName: this.form.controls.lastName.value ?? '',
        fligthNumber: this.form.controls.fligthNumber.value ?? 0,
        departureDate: this.form.controls.departureDate.value ?? '',
        landingDate: this.form.controls.landingDate.value ?? '',
        ticketClass: this.form.controls.ticketClass.value ?? 0,
      };
      this.reservationService.addReservation(createReservation).subscribe({
        next: (response) => {
          console.log('Reservation added:', response);
          this.onAddReservation.emit();
          this.onCloseForm.emit();
        },
        error: (err) => console.error('Error adding reservation:', err),
      });
    } else {
      const editReservation: Reservation = {
        id: this.form.controls.id.value ?? '',
        name: this.form.controls.name.value ?? '',
        lastName: this.form.controls.lastName.value ?? '',
        fligthNumber: this.form.controls.fligthNumber.value ?? 0,
        departureDate: this.form.controls.departureDate.value ?? '',
        landingDate: this.form.controls.landingDate.value ?? '',
        ticketClass: this.form.controls.ticketClass.value ?? 0,
      };
      this.reservationService.editReservation(editReservation).subscribe({
        next: (response) => {
          this.displayForm = false;
          this.onAddReservation.emit();
          this.onCloseForm.emit();
        },
        error: (err) => console.error('Error adding reservation:', err),
      });
    }

    this.resetForm();
  }

  resetForm(): void {
    this.form.reset({
      id: '',
      name: '',
      lastName: '',
      fligthNumber: 0,
      departureDate: '',
      landingDate: '',
      ticketClass: 0,
    });
  }
}
