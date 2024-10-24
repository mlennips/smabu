import { useState, useEffect } from 'react';
import { CreateOfferCommand, Currency, CustomerDTO, OfferId } from '../../types/domain';
import { Button, ButtonGroup, Grid2 as Grid, Paper, TextField } from '@mui/material';
import { deepValueChange } from '../../utils/deepValueChange';
import createId from '../../utils/createId';
import { useNavigate } from 'react-router-dom';
import { useNotification } from '../../contexts/notificationContext';
import DefaultContentContainer from '../../containers/DefaultContentContainer';
import { getCustomers } from '../../services/customer.service';
import { createOffer } from '../../services/offer.service';

const defaultCurrency: Currency = {
    isoCode: 'EUR',
    name: 'Euro',
    sign: '€'
};

const OfferCreate = () => {
    const [data, setData] = useState<CreateOfferCommand>({
        id: createId<OfferId>(),
        currency: defaultCurrency,
        customerId: { value: '' }
    });
    const [loading, setLoading] = useState(true);
    const [customers, setCustomers] = useState<CustomerDTO[]>();
    const [error, setError] = useState(null);
    const navigate = useNavigate();
    const { toast } = useNotification();

    useEffect(() => {
        getCustomers()
            .then(response => {
                setCustomers(response.data);
                setLoading(false);
            })
            .catch(error => {
                setError(error);
                setLoading(false);
            });
    }, []);

    const handleChange = (e: any) => {
        let { name, value } = e.target;
        if (name === 'customerId') { 
            value = { value: value };
        }
        setData(deepValueChange(data, name, value));
    };

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        createOffer({
            id: data!.id,
            customerId: data!.customerId,
            currency: data!.currency,
            taxRate: data!.taxRate
        })
            .then((_response) => {
                setLoading(false);
                toast("Angebot erfolgreich erstellt", "success");
                navigate(`/offers/${data.id.value}`);
            })
            .catch(error => {
                setError(error);
                setLoading(false);
            });
    };

    return (
        <form id="form" onSubmit={handleSubmit}>
            <Grid container spacing={2}>
                <Grid size={{ xs: 12 }}>
                    <DefaultContentContainer loading={loading} error={error} >
                        <Paper sx={{ p: 2 }}>
                            <Grid container spacing={1}>
                                <Grid size={{ xs: 6 }}><TextField fullWidth label="Währung" name="currency" value={data?.currency.isoCode} onChange={handleChange} required disabled /></Grid>
                                <Grid size={{ xs: 12 }}>
                                    <TextField select fullWidth label="Kunde" name="customerId"
                                        value={data?.customerId.value} onChange={handleChange} required
                                        slotProps={{
                                            select: {
                                                native: true,
                                            },
                                        }}
                                    >
                                        <option value="" disabled>
                                            Kunde auswählen
                                        </option>
                                        {customers?.map((customer) => (
                                            <option key={customer.id?.value} value={customer.id?.value}>
                                                {customer.name}
                                            </option>
                                        ))}
                                    </TextField>
                                </Grid>
                            </Grid>
                        </Paper>
                    </DefaultContentContainer >
                </Grid>
                <Grid size={{ xs: 12 }}>
                    <ButtonGroup>
                        <Button type="submit" variant="contained" color="success">
                            Erstellen
                        </Button>
                    </ButtonGroup>
                </Grid>
            </Grid>
        </form>
    );
};

export default OfferCreate;
