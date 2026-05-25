'use client';

import { useEffect, useRef, useState } from 'react';

export type FilterField =
  | { type: 'search'; key: string; placeholder: string; value: string }
  | {
      type: 'select';
      key: string;
      label: string;
      value: string;
      options: { value: string; label: string }[];
    }
  | { type: 'dateRange'; key: string; label: string; from: string; to: string }
  | { type: 'amountRange'; key: string; label: string; min: string; max: string };

export type FilterChangeValue =
  | string
  | { from?: string; to?: string }
  | { min?: string; max?: string };

export interface FilterPanelProps {
  fields: FilterField[];
  onChange: (key: string, value: FilterChangeValue) => void;
}

const INPUT_CLASS =
  'rounded-lg border border-gray-200 px-3 py-2 text-sm focus:border-pine-green focus:outline-none focus:ring-1 focus:ring-pine-green';

/**
 * Debounced search input — fires onChange ~400ms after the user stops typing.
 * Extracted so each search field owns its own debounce timer/state.
 */
function SearchField({
  field,
  onChange,
}: {
  field: Extract<FilterField, { type: 'search' }>;
  onChange: (key: string, value: FilterChangeValue) => void;
}) {
  const [value, setValue] = useState(field.value);
  const debounceRef = useRef<ReturnType<typeof setTimeout> | null>(null);

  useEffect(() => {
    if (debounceRef.current) clearTimeout(debounceRef.current);
    debounceRef.current = setTimeout(() => onChange(field.key, value), 400);
    return () => {
      if (debounceRef.current) clearTimeout(debounceRef.current);
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [value]);

  return (
    <input
      type="search"
      value={value}
      onChange={(e) => setValue(e.target.value)}
      placeholder={field.placeholder}
      aria-label={field.placeholder}
      className={`${INPUT_CLASS} w-full max-w-sm`}
    />
  );
}

/**
 * Reusable, generic filter bar. Renders search / select / date-range / amount-range
 * fields from a small config. Search is debounced internally (~400ms); other types
 * fire immediately. Presentational — the parent owns the resulting filter state.
 */
export function FilterPanel({ fields, onChange }: FilterPanelProps) {
  return (
    <div className="flex flex-wrap items-end gap-4">
      {fields.map((field) => {
        switch (field.type) {
          case 'search':
            return <SearchField key={field.key} field={field} onChange={onChange} />;

          case 'select':
            return (
              <label key={field.key} className="flex flex-col gap-1 text-xs text-gray-500">
                {field.label}
                <select
                  value={field.value}
                  onChange={(e) => onChange(field.key, e.target.value)}
                  aria-label={field.label}
                  className={INPUT_CLASS}
                >
                  {field.options.map((opt) => (
                    <option key={opt.value} value={opt.value}>
                      {opt.label}
                    </option>
                  ))}
                </select>
              </label>
            );

          case 'dateRange':
            return (
              <fieldset key={field.key} className="flex flex-col gap-1 text-xs text-gray-500">
                <span>{field.label}</span>
                <div className="flex items-center gap-2">
                  <input
                    type="date"
                    value={field.from}
                    onChange={(e) => onChange(field.key, { from: e.target.value, to: field.to })}
                    aria-label={`${field.label} from`}
                    className={INPUT_CLASS}
                  />
                  <input
                    type="date"
                    value={field.to}
                    onChange={(e) => onChange(field.key, { from: field.from, to: e.target.value })}
                    aria-label={`${field.label} to`}
                    className={INPUT_CLASS}
                  />
                </div>
              </fieldset>
            );

          case 'amountRange':
            return (
              <fieldset key={field.key} className="flex flex-col gap-1 text-xs text-gray-500">
                <span>{field.label}</span>
                <div className="flex items-center gap-2">
                  <input
                    type="number"
                    value={field.min}
                    onChange={(e) => onChange(field.key, { min: e.target.value, max: field.max })}
                    aria-label={`${field.label} min`}
                    className={`${INPUT_CLASS} w-28`}
                  />
                  <input
                    type="number"
                    value={field.max}
                    onChange={(e) => onChange(field.key, { min: field.min, max: e.target.value })}
                    aria-label={`${field.label} max`}
                    className={`${INPUT_CLASS} w-28`}
                  />
                </div>
              </fieldset>
            );

          default:
            return null;
        }
      })}
    </div>
  );
}
