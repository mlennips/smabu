import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { Button, ButtonGroup, Grid2 as Grid, Paper, TextField } from '@mui/material';
import DefaultContentContainer, { ToolbarItem } from '../../containers/DefaultContentContainer';
import { deepValueChange } from '../../utils/deepValueChange';
import { Delete } from '@mui/icons-material';
import { useNotification } from '../../contexts/notificationContext';
import { getQuantityUnits } from '../../services/common.service';
import { getOffer, updateOfferItem } from '../../services/offer.service';
import { OfferDTO, OfferItemDTO } from '../../types/domain';

const OfferItemDetails = () => {
    const params = useParams();
    const { toast } = useNotification();
    const [offer, setOffer] = useState<OfferDTO>();
    const [data, setData] = useState<OfferItemDTO>();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [units, setUnits] = useState<string[]>([]);

    const loadData = () => getOffer(params.offerId!, true)
        .then(response => {
            setOffer(response.data);
            setData(response.data.items?.find((item: OfferItemDTO) => item.id!.value === params.id));
            setLoading(false);
        })
        .catch(error => {
            setError(error);
            setLoading(false);
        });

    useEffect(() => {
        loadData();

        getQuantityUnits()
        .then(response => {
            setUnits(response);
        })
        .catch(error => {
            setError(error);
        });
    }, []);

    const handleChange = (e: any) => {
        const { name, value } = e.target;
        setData(deepValueChange(data, name, value));
    };

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        setLoading(true);
        updateOfferItem(params.offerId!, params.id!, {
            id: data?.id!,
            offerId: data?.offerId!,
            quantity: data?.quantity!,
            unitPrice: data?.unitPrice!,
            details: data?.details!            
        })
            .then(() => {
                setLoading(false);
                setError(null);
                toast("Angebotsposition erfolgreich gespeichert", "success");
                loadData();
            })
            .catch(error => {
                setError(error);
                setLoading(false);
            });
    };

    const toolbarItems: ToolbarItem[] = [
        {
            text: "Löschen",
            route: `/offers/${params.offerId}/items/${data?.id?.value}/delete`,
            icon: <Delete />
        }
    ];

    return (
        <form id="form" onSubmit={handleSubmit}>
        <Grid container spacing={2}>
            <Grid size={{ xs: 12 }}>
                    <DefaultContentContainer title={offer?.displayName} subtitle={"#" + data?.displayName} loading={loading} error={error} toolbarItems={toolbarItems} >
                        <Paper sx={{ p: 2 }}>
                            <Grid container spacing={2}>
                                <Grid size={{ xs: 12, sm: 2, md: 2 }}><TextField fullWidth label="Position" name="position" value={data?.position} disabled /></Grid>
                                <Grid size={{ xs: 12, sm: 5, md: 4 }}><TextField fullWidth label="Angebot" name="offer" value={offer?.displayName} disabled /></Grid>
                                <Grid size={{ xs: 12, sm: 5, md: 6 }}><TextField fullWidth label="Kunde" name="customer" value={offer?.customer?.name} disabled /></Grid>

                                <Grid size={{ xs: 6, sm: 6, md: 3 }}><TextField type='number' fullWidth label="Anzahl" name="quantity.value" value={data?.quantity?.value} onChange={handleChange} required /></Grid>
                                <Grid size={{ xs: 6, sm: 6, md: 3 }}>
                                    <TextField select fullWidth label="Einheit"  name="quantity.unit"
                                        value={data?.quantity?.unit}  onChange={handleChange} required
                                        slotProps={{
                                            select: {
                                                native: true,
                                            }
                                        }}                                        
                                    >
                                        <option value="" />
                                        {units.map((unit) => (
                                            <option key={unit} value={unit}>
                                                {unit}
                                            </option>
                                        ))}
                                    </TextField>
                                </Grid>
                                <Grid size={{ xs: 6, sm: 6, md: 3 }}><TextField type='number' fullWidth label="Einzelpreis" name="unitPrice" value={data?.unitPrice} onChange={handleChange} required /></Grid>
                                <Grid size={{ xs: 6, sm: 6, md: 3 }}><TextField type='number' fullWidth label="Gesamt" name="totalPrice" value={data?.totalPrice} disabled /></Grid>
                            </Grid>
                        </Paper>
                    </DefaultContentContainer >
            </Grid>

            <Grid size={{ xs: 12 }}>
                <DefaultContentContainer title="Details" loading={loading}>
                    <TextField multiline variant='filled' minRows={5} maxRows={10} fullWidth 
                        name="details" value={data?.details} onChange={handleChange} />
                </DefaultContentContainer>
            </Grid>

            <Grid size={{ xs: 12 }}>
                <ButtonGroup disabled={loading}>
                    <Button type="submit" variant="contained" form="form" color="success">
                        Speichern
                    </Button>
                </ButtonGroup>
            </Grid>
        </Grid>
        </form>
    );
};

export default OfferItemDetails;


