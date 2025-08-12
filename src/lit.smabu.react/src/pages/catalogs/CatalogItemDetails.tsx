import { useState, useEffect } from 'react';
import { CatalogItemDTO, CatalogItemPrice, CustomerDTO } from '../../types/domain';
import { useParams } from 'react-router-dom';
import { Grid, IconButton, InputAdornment, Paper, Stack, TextField } from '@mui/material';
import DefaultContentContainer, { ToolbarItem } from '../../components/contentBlocks/DefaultContentBlock';
import { deepValueChange } from '../../utils/deepValueChange';
import { useNotification } from '../../contexts/notificationContext';
import { DetailsActions } from '../../components/contentBlocks/PageActionsBlock';
import { getCatalogItem, updateCatalogItem } from '../../services/catalogs.service';
import { UnitSelectField } from '../../components/controls/SelectField';
import { Add, Cancel, RemoveCircle, ToggleOff, ToggleOn } from '@mui/icons-material';
import { formatForTextField } from '../../utils/formatDate';
import { getCustomers } from '../../services/customer.service';
import { handleAsyncTask } from '../../utils/handleAsyncTask';

const CatalogItemDetails = () => {
    const params = useParams();
    const { toast } = useNotification();
    const [data, setData] = useState<CatalogItemDTO>();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const [customers, setCustomers] = useState<CustomerDTO[]>([]);
    const toolbarDetails: ToolbarItem[] = [
        {
            text: data?.isActive ? "Aktiviert" : "Deaktiviert",
            color: data?.isActive ? "success" : "error",
            icon: data?.isActive ? <ToggleOn /> : <ToggleOff />,
            action: () => {
                setData(deepValueChange(data, 'isActive', !data?.isActive))
            }
        }
    ];

    useEffect(() => {
        loadData();

        handleAsyncTask({
            task: getCustomers,
            onLoading: setLoading,
            onSuccess: setCustomers,
            onError: setError
        });
    }, []);

    const loadData = () => handleAsyncTask({
        task: () => getCatalogItem(params.catalogId!, params.catalogItemId!),
        onLoading: setLoading,
        onSuccess: setData,
        onError: setError
    });

    const handleChange = (e: any) => {
        const { name, value } = e.target;
        setData(deepValueChange(data, name, value));
    };

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        handleAsyncTask({
            task: () => updateCatalogItem(params.catalogId!, params.catalogItemId!, {
                catalogItemId: data?.id!,
                catalogId: data?.catalogId!,
                name: data?.name!,
                description: data?.description!,
                isActive: data?.isActive!,
                unit: data?.unit!,
                prices: data?.prices!,
                customerPrices: data?.customerPrices!
            }),
            onLoading: setLoading,
            onSuccess: () => {
                toast("Erfolgreich gespeichert", "success");
            },
            onError: setError
        });
    };

    return (
        <form id="form" onSubmit={handleSubmit} >
            <Stack spacing={2}>
                {renderDetails(data, loading, error, toolbarDetails, handleChange)}
                {renderPrices(data, setData, customers)}
                <DetailsActions formId="form" deleteUrl={`/catalogs/${data?.catalogId?.value}/items/${data?.id?.value}/delete`} disabled={loading} />
            </Stack>
        </form>
    );
};

const renderDetails = (data: CatalogItemDTO | undefined, loading: boolean, error: undefined, toolbarDetails: ToolbarItem[], handleChange: (e: any) => void) => {
    return <DefaultContentContainer subtitle={`${data?.displayName} (${data?.groupName})`} loading={loading} error={error} toolbarItems={toolbarDetails}>
        <Paper sx={{ p: 2 }}>
            <Grid container spacing={2}>
                <Grid size={{ xs: 12, sm: 2 }}>
                    <TextField
                        fullWidth
                        label="#"
                        name="number"
                        value={data?.number?.displayName || ''}
                        disabled />
                </Grid>
                <Grid size={{ xs: 12, sm: 8 }}>
                    <TextField
                        fullWidth
                        label="Name"
                        name="name"
                        value={data?.name || ''}
                        onChange={handleChange} />
                </Grid>
                <Grid size={{ xs: 6, sm: 2 }}>
                    <UnitSelectField name="unit" label='Einheit' value={data?.unit?.value} required
                        onChange={handleChange} />
                </Grid>
                <Grid size={{ xs: 12, sm: 12 }}>
                    <TextField
                        fullWidth multiline
                        rows={4}
                        label="Description"
                        name="description"
                        value={data?.description || ''}
                        onChange={handleChange} />
                </Grid>
            </Grid>
        </Paper>
    </DefaultContentContainer>;
}

const renderPrices = (data: CatalogItemDTO | undefined, setData: any, customers: CustomerDTO[]) => {
    return <Grid container spacing={2}>
        <Grid size={{ xs: 12, sm: 6 }}>
            {renderDefaultPrices(data, setData)}
        </Grid>
        <Grid size={{ xs: 12, sm: 6 }}>
            {renderCustomerPrices(data, setData, customers)}
        </Grid>
    </Grid>;
}

const renderDefaultPrices = (data: CatalogItemDTO | undefined, setData: any) => {
    const toolbar: ToolbarItem[] = [
        {
            text: "Neuer Preis",
            icon: <Add />,
            action: () => {
                const newPrice = {
                    price: 0,
                    validFrom: new Date()
                };
                setData(deepValueChange(data, 'prices', [newPrice, ...data!.prices || []]));
            }
        }
    ];

    const handleChange = (item: CatalogItemPrice, e: any) => {
        const { value, name } = e.target;
        if (name === 'price') {
            item.price = parseFloat(value);
            setData(deepValueChange(data, 'prices', data!.prices));
        }
        if (name === 'validFrom') {
            item.validFrom = new Date(value);
            setData(deepValueChange(data, 'prices', data!.prices));
        }
    }

    return <DefaultContentContainer title="Standardpreise" loading={!data} toolbarItems={toolbar}>
        <Paper sx={{ p: 2 }}>
            <Stack>
                {data?.prices?.map((price, index) => (
                    <Grid container spacing={2} key={index}>
                        <Grid size={{ xs: "grow" }}>
                            <TextField
                                fullWidth
                                type="number"
                                label="Preis"
                                name="price"
                                slotProps={{
                                    input: {
                                        startAdornment: <InputAdornment position="start">{price.currency?.isoCode}</InputAdornment>,
                                        inputMode: 'decimal'
                                    }
                                }}
                                onChange={(e) => handleChange(price, e)}
                                value={price.price} />
                        </Grid>
                        <Grid size={{ xs: "auto" }}>
                            <TextField
                                fullWidth
                                label="Gültig ab"
                                name="validFrom"
                                type="datetime-local"
                                onChange={(e) => handleChange(price, e)}
                                value={formatForTextField(price.validFrom, 'datetime')} />
                        </Grid>
                        <Grid size={{ xs: "auto" }} sx={{ display: 'flex', alignItems: 'self-end' }}>
                            <IconButton size='small' onClick={() => {
                                const updatedPrices = data!.prices!.filter((_, i) => i !== index);
                                setData(deepValueChange(data, 'prices', updatedPrices));
                            }}>
                                <RemoveCircle />
                            </IconButton>
                        </Grid>
                    </Grid>
                ))}
            </Stack>
        </Paper>
    </DefaultContentContainer>;
}

function renderCustomerPrices(data: CatalogItemDTO | undefined, setData: any, customers: CustomerDTO[]) {
    const [isAddKunde, setIsAddKunde] = useState<boolean>(false);

    const handleChange = (item: CatalogItemPrice, e: any) => {
        const { value, name } = e.target;
        if (name === 'price') {
            item.price = parseFloat(value);
            setData(deepValueChange(data, 'customerPrices', data!.customerPrices));
        }
    }

    const toolbar: ToolbarItem[] = [
        {
            text: "Neuer Kundenpreis",
            icon: <Add />,
            action: () => {
                setIsAddKunde(true);
            }
        }
    ];

    return <DefaultContentContainer title="Kundenpreise" loading={!data} toolbarItems={toolbar}>
        <Paper sx={{ p: 2 }}>
            <Stack spacing={2}>
                {isAddKunde && <Grid container>
                    <Grid size={{ xs: 'grow' }} >
                        <TextField
                            select
                            fullWidth
                            label="Kunde hinzufügen"
                            name="customer"
                            slotProps={{
                                select: {
                                    native: true,
                                }
                            }}
                            onChange={(e) => {
                                const customerId = e.target.value;
                                if (data?.customerPrices && data.customerPrices.filter(x => x.customerId?.value === customerId).length === 0) {
                                    const newPrice = {
                                        price: 0,
                                        validFrom: new Date(),
                                        customerId: { value: customerId },
                                    };
                                    data.customerPrices = [newPrice, ...data.customerPrices];
                                }
                                setData(deepValueChange(data, 'customerPrices', data!.customerPrices));
                                setIsAddKunde(false);
                            }}
                        >
                            <option value="">Bitte wählen</option>
                            {customers.map((customer) => (
                                <option key={customer.id?.value} value={customer.id?.value}>
                                    {customer.name}
                                </option>
                            ))}
                        </TextField>
                    </Grid>
                    <Grid size={{ xs: 'auto' }} sx={{ display: 'flex', alignItems: 'self-end' }}>
                        <IconButton size='small' onClick={() => setIsAddKunde(false)}>
                            <Cancel />
                        </IconButton>
                    </Grid>
                </Grid>}

                {data?.customerPrices && customers && data.customerPrices.map(customerPrice => {
                    const customer = customers.find(c => c.id?.value === customerPrice.customerId?.value);
                    return <Grid container spacing={2} key={customerPrice.customerId?.value}>
                        <Grid size={{ xs: 'grow' }}>
                            <TextField
                                fullWidth
                                type="number"
                                label={customer?.name}
                                name="price"
                                slotProps={{
                                    input: {
                                        startAdornment: <InputAdornment position="start">{customer?.currency?.isoCode}</InputAdornment>,
                                        inputMode: 'decimal'
                                    }
                                }}
                                onChange={(e) => handleChange(customerPrice, e)}
                                value={customerPrice?.price || 0} />
                        </Grid>
                        <Grid size={{ xs: "auto" }} sx={{ display: 'flex', alignItems: 'self-end' }}>
                            <IconButton size='small' onClick={() => {
                                const updatedCustomerPrices = data!.customerPrices?.filter(x => x.customerId?.value != customerPrice.customerId?.value);
                                setData(deepValueChange(data, 'customerPrices', updatedCustomerPrices));
                            }}>
                                <RemoveCircle />
                            </IconButton>
                        </Grid>
                    </Grid>
                })}
            </Stack>
        </Paper>
    </DefaultContentContainer>;
}

export default CatalogItemDetails;
