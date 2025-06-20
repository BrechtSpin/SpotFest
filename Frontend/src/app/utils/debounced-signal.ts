import { effect, signal, type Signal } from '@angular/core';

export function debouncedSignal<T>(
  sourceSignal: Signal<T>,
  debounceTimeInMs = 0
): Signal<T> {
  const debounceSignal = signal(sourceSignal());
  effect(
    (onCleanup) => {
      const value = sourceSignal();
      const timeout = setTimeout(() => {
        if (value !== debounceSignal()) {
          debounceSignal.set(value);
        }
      }, debounceTimeInMs);
      onCleanup(() => clearTimeout(timeout));
    },
    { allowSignalWrites: true }
  );
  return debounceSignal;
}
