import { Component, forwardRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-currency-input',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CurrencyInputComponent),
      multi: true
    }
  ],
  template: `
    <mat-form-field appearance="outline" class="w-full">
      <mat-label>金額</mat-label>
      <input matInput type="number" [value]="value" (input)="onInput($event)" placeholder="0">
      <span matPrefix>￥&nbsp;</span>
    </mat-form-field>
  `,
  styles: [`.w-full { width: 100%; }`]
})
export class CurrencyInputComponent implements ControlValueAccessor {
  value: number = 0;
  onChange: any = () => {};
  onTouched: any = () => {};

  writeValue(value: any): void {
    this.value = value;
  }
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  onInput(event: any): void {
    const val = event.target.value;
    this.value = val;
    this.onChange(val);
  }
}
