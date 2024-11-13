import React, { useEffect, useState } from 'react';
import { GetOrderReferencesResponse, InvoiceId, InvoiceIdOrderReferenceDTO, OfferId, OfferIdOrderReferenceDTO } from '../../types/domain';
import { Chip, Grid2 as Grid, Stack } from '@mui/material';
import { Cancel, Edit, Save } from '@mui/icons-material';
import { useNotification } from '../../contexts/notificationContext';
import { ToolbarItem } from '../../components/contentBlocks/DefaultContentBlock';
import { getOrdersReferences, updateOrderReferences } from '../../services/order.service';
import { handleAsyncTask } from '../../utils/handleAsyncTask';

interface OrderReferencesComponentProps {
    orderId: string;
    setError: (error: any) => void | undefined;
    setToolbar?: (items: ToolbarItem[]) => void | undefined;
    setLoading?: (loading: boolean) => void | undefined;
}

const OrderReferencesComponent: React.FC<OrderReferencesComponentProps> = ({ orderId, setError, setToolbar, setLoading }) => {
    const [data, setData] = useState<GetOrderReferencesResponse>();
    const [addMode, setAddMode] = useState(false);
    const { toast } = useNotification();
    const toolbar: ToolbarItem[] = []
    if (addMode) {
        toolbar.push(
            {
                text: "Abbrechen",
                icon: <Cancel />,
                action: () => { setAddMode(false) },
            });
        toolbar.push(
            {
                text: "Speichern",
                icon: <Save />,
                action: () => submit(),
            });
    } else {
        toolbar.push(
            {
                text: "Bearbeiten",
                icon: <Edit />,
                action: () => { setAddMode(true) },
            },
        );
    }

    useEffect(() => {
        setLoading && setLoading(true);
        loadData();
        setToolbar && setToolbar(toolbar);
    }, [addMode, orderId]);
    
    const loadData = () => handleAsyncTask({
        task: () => getOrdersReferences(orderId!),
        onLoading: setLoading,
        onSuccess: setData,
        onError: setError
    });

    const submit = () => handleAsyncTask({
        task: () => updateOrderReferences(orderId!, {
            orderId: { value: orderId! },
            references: {
                offerIds: data!.offers?.filter(x => x.isSelected).map(x => x.id!),
                invoiceIds: data!.invoices?.filter(x => x.isSelected).map(x => x.id!)
            }
        }),
        onLoading: setLoading,
        onSuccess: () => {
            setAddMode(false)
            toast("Erledigt", "success");
        },
        onError: setError
    });

    const addReference = (id: InvoiceId | OfferId) => {
        const updatedData = { ...data! };
        if (data?.offers?.find(x => x.id?.value === id.value)) {
            updatedData.offers = updatedData.offers?.map(x => {
                if (x.id === id) {
                    x.isSelected = !x.isSelected;
                }
                return x;
            });
        } else {
            updatedData.invoices = updatedData.invoices?.map(x => {
                if (x.id === id) {
                    x.isSelected = !x.isSelected;
                }
                return x;
            });
        }
        setData(updatedData);
        setToolbar && setToolbar(toolbar);
    }

    return (
        <Grid container spacing={2}>
            {data?.offers && (
                <Grid size={{ xs: 12 }}>
                    {data.offers && renderReferenceType(data.offers!, "offer", "Angebote", "/offers",
                        data.offerAmount, addMode, addReference)}
                </Grid>
            )}
            {data?.invoices && (
                <Grid size={{ xs: 12 }}>
                    {data.invoices && renderReferenceType(data.invoices!, "invoice", "Rechnungen", "/invoices",
                        data.invoiceAmount, addMode, addReference)}
                </Grid>
            )}
        </Grid>
    );
};

const renderReferenceType = (
    data: OfferIdOrderReferenceDTO[] | InvoiceIdOrderReferenceDTO[], key: string, label: string, url: string, amount: number | undefined, addMode: boolean,
    addReference: (id: InvoiceId | OfferId) => void) => {
    return (
        <Stack direction="row" spacing={1} useFlexGap
            sx={{
                p: 0,
                justifyContent: "flex-start",
                flexWrap: 'wrap'
            }}>
            <Chip key={key} clickable label={label}
                variant='outlined' component="a" href={url}
                sx={{ width: "110px", fontWeight: 600 }} />

            {!addMode && amount != undefined && amount > 0 && <Chip key={"Amount"}
                variant='outlined'
                sx={{ width: "110px" }}
                label={amount.toFixed(2)} />}

            {!addMode && data.filter(x => x.isSelected).length === 0 && (
                <Chip key={"NoItems" + key} variant='outlined' label={"Keine Verknüpfungen"} />
            )}

            {data?.filter(x => addMode ? true : x.isSelected).map((reference) => {
                return addMode ? (
                    <Chip clickable key={reference.id?.value}
                        label={reference.name}
                        variant={reference.isSelected ? "filled" : "outlined"}
                        onClick={() => addReference(reference.id!)} />
                ) : (
                    <Chip key={reference.id?.value}
                        clickable
                        component={"a"}
                        sx={{ width: "110px" }}
                        href={url + "/" + reference.id?.value}
                        label={reference.name} />
                );
            })}
        </Stack>
    );;
}

export default OrderReferencesComponent;