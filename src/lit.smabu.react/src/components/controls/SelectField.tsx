import { TextField } from '@mui/material';
import React, { useEffect, useState } from 'react';
import { getPaymentMethods, getUnits } from '../../services/common.service';
import { PaymentMethod, Unit } from '../../types/domain';
import { Label } from '@mui/icons-material';

interface SelectFieldProps<T> {
    label: string;
    name: string;
    items: T[];
    value: any | null | undefined;
    required?: boolean;
    onChange: (event: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => void;
    onGetValue: (item: T) => string;
    onGetLabel: (item: T) => string;
}

interface TypedSelectFieldProps {
    label: string;
    name: string;
    value: string | null | undefined;
    required: boolean;
    onChange: (event: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => void;
}

export const PaymentMethodSelectField: React.FC<TypedSelectFieldProps> = ({ name, label, value, required, onChange }) => {
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const [units, setUnits] = useState<PaymentMethod[]>([]);

    useEffect(() => {
        getPaymentMethods()
            .then(response => {
                setLoading(false);
                setUnits(response);
            })
            .catch(error => {
                setLoading(false);
                setError(error);
            });
    }, []);

    if (units && !loading && !error) {
        return <SelectField items={units} label={label} name={name} value={value}
            required={required} onChange={onChange}
            onGetLabel={(item) => item.value}
            onGetValue={(item) => item.value} />;
    } else if (loading) {
        return <TextField label={label} name={name} value={"..."} disabled={true} />
    } else {
        return <TextField label={label} name={name} value={error} disabled={true} color='error' />
    }
}

export const UnitSelectField: React.FC<TypedSelectFieldProps> = ({ name, label, value, required, onChange }) => {
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const [units, setUnits] = useState<Unit[]>([]);

    useEffect(() => {
        getUnits()
            .then(response => {
                setLoading(false);
                setUnits(response);
            })
            .catch(error => {
                setLoading(false);
                setError(error);
            });
    }, []);

    if (units && !loading && !error) {
        return <SelectField items={units} label={label} name={name} value={value}
            required={required} onChange={onChange}
            onGetLabel={(item) => item.name}
            onGetValue={(item) => item.value} />;
    } else if (loading) {
        return <TextField label={label} name={name} value={"..."} disabled={true} />
    } else {
        return <TextField label={label} name={name} value={error} disabled={true} color='error' />
    }
}

const SelectField: React.FC<SelectFieldProps<any>> = ({ items, label, name, value, required, onChange, onGetLabel, onGetValue }) => {
    const onPrepareChange = (e: any) => {
        let { name: targetName, value: targetValue } = e.target;
        if (targetName === name) {
            targetValue = items.find(i => i.value === targetValue || onGetValue(i) === targetValue);
            if (targetValue && targetValue.id) {
                targetValue = targetValue.id;
            }
        }
        onChange({ target: { name: targetName, value: targetValue } } as any);
    }

    return (
        <TextField select fullWidth label={label} name={name}
            value={value} onChange={onPrepareChange} required={required}
            slotProps={{
                select: {
                    native: true,
                }
            }}>
            <option key="leer" value={undefined}>
                Bitte auswählen
            </option>
            {items.map((item) => (
                <option key={onGetValue(item)} value={onGetValue(item)}>
                    {onGetLabel(item)}
                </option>
            ))}
        </TextField>
    );
};

export default SelectField;