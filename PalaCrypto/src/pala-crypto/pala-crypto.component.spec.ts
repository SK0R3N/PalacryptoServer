import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PalaCryptoComponent } from './pala-crypto.component';

describe('PalaCryptoComponent', () => {
  let component: PalaCryptoComponent;
  let fixture: ComponentFixture<PalaCryptoComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PalaCryptoComponent]
    });
    fixture = TestBed.createComponent(PalaCryptoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
