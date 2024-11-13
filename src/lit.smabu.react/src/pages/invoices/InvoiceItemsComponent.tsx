import React, { useEffect, useState } from 'react';
import { InvoiceDTO, InvoiceItemId } from '../../types/domain';
import { Button, ButtonGroup, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from '@mui/material';
import { Add, Delete, Edit, MoveDown, MoveUp } from '@mui/icons-material';
import { useNotification } from '../../contexts/notificationContext';
import { ToolbarItem } from '../../components/contentBlocks/DefaultContentBlock';
import { getInvoice, moveInvoiceItemDown, moveInvoiceItemUp } from '../../services/invoice.service';
import { handleAsyncTask } from '../../utils/handleAsyncTask';
import { grey } from '@mui/material/colors';

interface InvoiceItemsComponentProps {
    invoiceId: string | undefined;
    setError: (error: any) => void;
    setToolbar?: (items: ToolbarItem[]) => void;
}

const InvoiceItemsComponent: React.FC<InvoiceItemsComponentProps> = ({ invoiceId, setError, setToolbar }) => {
    const [data, setData] = useState<InvoiceDTO>();
    const { toast } = useNotification();
    const toolbarItemsItems: ToolbarItem[] = [
        {
            text: "Neu",
            route: `/invoices/${invoiceId}/items/create`,
            icon: <Add />
        }
    ];

    useEffect(() => {
        loadData();
        setToolbar && setToolbar(toolbarItemsItems);
    }, [invoiceId]);

    const loadData = () => handleAsyncTask({
        task: () => getInvoice(invoiceId!, true),
        onSuccess: setData,
        onError: (error) => {
            setError(error);
        }
    });

    const moveItemUp = (itemId: InvoiceItemId) => handleAsyncTask({
        task: () => moveInvoiceItemUp(data?.id?.value!, itemId.value!),
        onSuccess: () => {
            toast("Position nach oben verschoben", "success");
            loadData();
        },
        onError: (error) => {
            setError(error);
        }
    });

    const moveItemDown = (itemId: InvoiceItemId) => handleAsyncTask({
        task: () => moveInvoiceItemDown(data?.id?.value!, itemId.value!),
        onSuccess: () => {
            toast("Position nach unten verschoben", "success");
            loadData();
        },
        onError: (error) => {
            setError(error);
        }
    });

    return (
        <TableContainer sx={{ p: 0 }}>
            <Table size='small' >
                <TableHead>
                    <TableRow>
                        <TableCell variant='head'><b>Pos.</b></TableCell>
                        <TableCell variant='head'></TableCell>
                        <TableCell variant='head' align="right" style={{ width: 'auto' }}><b>Menge</b></TableCell>
                        <TableCell variant='head' align="right" style={{ width: 'auto' }}><b>Preis</b></TableCell>
                        <TableCell variant='head' align="right" style={{ width: 'auto' }}><b>Gesamt</b></TableCell>
                        <TableCell variant='head'></TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {data?.items?.map((item, index) => (
                        <React.Fragment key={`${index}-1`}>
                            <TableRow key={index}>
                                <TableCell sx={{ borderBottom: 'none' }}>{item.position}</TableCell>
                                <TableCell component="th" scope="row" sx={{ borderBottom: 'none' }}></TableCell>
                                <TableCell sx={{ borderBottom: 'none' }} align="right">{item.quantity?.value} {item.quantity?.unit?.shortName}</TableCell>
                                <TableCell sx={{ borderBottom: 'none' }} align="right">{item.unitPrice?.toFixed(2)} {data?.currency?.isoCode}</TableCell>
                                <TableCell sx={{ borderBottom: 'none' }} align="right">{item.totalPrice?.toFixed(2)} {data?.currency?.isoCode}</TableCell>
                                <TableCell sx={{ verticalAlign: 'top' }} rowSpan={2} align="right" >
                                    <ButtonGroup variant="text" size='small' color='secondary'>
                                        <Button onClick={() => moveItemUp(item.id!)} disabled={data.isReleased}><MoveUp /></Button>
                                        <Button onClick={() => moveItemDown(item.id!)} disabled={data.isReleased}><MoveDown /></Button>
                                        <Button LinkComponent="a" href={`/invoices/${data?.id?.value}/items/${item.id?.value}`}><Edit /></Button>
                                        <Button LinkComponent="a" color='error' href={`/invoices/${data?.id?.value}/items/${item.id?.value}/delete`} disabled={data.isReleased}><Delete /></Button>
                                    </ButtonGroup>
                                </TableCell>
                            </TableRow>
                            <TableRow key={`${index}-2`}>
                                <TableCell colSpan={6} sx={{ fontSize: 'small', color: grey[600] }}>{item.details}</TableCell>
                            </TableRow>
                        </React.Fragment>
                    ))}

                    <TableRow key="last">
                        <TableCell></TableCell>
                        <TableCell></TableCell>
                        <TableCell></TableCell>
                        <TableCell></TableCell>
                        <TableCell variant='footer' align="right" style={{ fontWeight: 'bold' }}>{data?.amount?.toFixed(2)} {data?.currency?.isoCode}</TableCell>
                        <TableCell></TableCell>
                    </TableRow>
                </TableBody>
            </Table>
        </TableContainer>
    );
};

export default InvoiceItemsComponent;