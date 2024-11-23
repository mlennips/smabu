import { TextField } from '@mui/material';
import React, { useEffect, useState } from 'react';
import { getFinancialCategories, getPaymentConditions, getPaymentMethods, getUnits } from '../../services/common.service';
import { PaymentCondition, PaymentMethod, Unit } from '../../types/domain';

interface SelectFieldProps<T> {
    label: string;
    name: string;
    items: T[];
    value: any | null | undefined;
    required?: boolean | undefined;
    disabled?: boolean | undefined;
    onChange: (event: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => void;
    onGetValue: (item: T) => string;
    onGetLabel: (item: T) => string;
    slotProps?: any;
}

interface TypedSelectFieldProps {
    label: string;
    name: string;
    value: string | null | undefined;
    required: boolean;
    disabled?: boolean | undefined;
    onChange: (event: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => void;
    slotProps?: any;
}

interface FinancialCategorySelectFieldProps extends TypedSelectFieldProps {
    type: 'incomes' | 'expenditures';
}

export const PaymentConditionSelectField: React.FC<TypedSelectFieldProps> = ({ name, label, value, required, onChange, slotProps, disabled }) => {
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const [units, setUnits] = useState<PaymentCondition[]>([]);

    useEffect(() => {
        getPaymentConditions()
            .then(response => {
                setLoading(false);
                setUnits(response);
            })
            .catch(error => {
                setLoading(false);
                setError(error);
            });
    }, [name]);

    if (units && !loading && !error) {
        return <SelectField items={units} label={label} name={name} value={value}
            required={required} onChange={onChange}
            disabled={disabled}
            onGetLabel={(item) => item.name}
            onGetValue={(item) => item.value}
            slotProps={slotProps} />;
    } else if (loading) {
        return <TextField label={label} name={name} value={"..."} disabled={true} />
    } else {
        return <TextField label={label} name={name} value={error} disabled={true} color='error' />
    }
}

export const PaymentMethodSelectField: React.FC<TypedSelectFieldProps> = ({ name, label, value, required, onChange, slotProps, disabled }) => {
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
    }, [name]);

    if (units && !loading && !error) {
        return <SelectField items={units} label={label} name={name} value={value}
            required={required} onChange={onChange}
            disabled={disabled}
            onGetLabel={(item) => item.name}
            onGetValue={(item) => item.value}
            slotProps={slotProps} />;
    } else if (loading) {
        return <TextField label={label} name={name} value={"..."} disabled={true} />
    } else {
        return <TextField label={label} name={name} value={error} disabled={true} color='error' />
    }
}

export const UnitSelectField: React.FC<TypedSelectFieldProps> = ({ name, label, value, required, onChange, disabled }) => {
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
    }, [name]);

    if (units && !loading && !error) {
        return <SelectField items={units} label={label} name={name} value={value}
            required={required} onChange={onChange}
            disabled={disabled}
            onGetLabel={(item) => item.name}
            onGetValue={(item) => item.value} />;
    } else if (loading) {
        return <TextField label={label} name={name} value={"..."} disabled={true} />
    } else {
        return <TextField label={label} name={name} value={error} disabled={true} color='error' />
    }
}

export const FinancialCategorySelectField: React.FC<FinancialCategorySelectFieldProps> = ({ name, label, value, required, onChange, type, slotProps, disabled }) => {
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const [units, setUnits] = useState<Unit[]>([]);

    useEffect(() => {
        getFinancialCategories(type)
            .then(response => {
                setLoading(false);
                setUnits(response);
            })
            .catch(error => {
                setLoading(false);
                setError(error);
            });
    }, [name]);

    if (units && !loading && !error) {
        return <SelectField items={units} label={label} name={name} value={value}
            required={required} onChange={onChange}
            disabled={disabled}
            onGetLabel={(item) => item.name}
            onGetValue={(item) => item.value}
            slotProps={slotProps}
        />;
    } else if (loading) {
        return <TextField label={label} name={name} value={"..."} disabled={true} />
    } else {
        return <TextField label={label} name={name} value={error} disabled={true} color='error' />
    }
}

const SelectField: React.FC<SelectFieldProps<any>> = ({ items, label, name, value, required, onChange, onGetLabel, onGetValue, slotProps, disabled }) => {
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
            disabled={disabled ?? false}
            slotProps={{
                ...slotProps,
                select: {
                    native: true,
                    ...(slotProps?.select || {})
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