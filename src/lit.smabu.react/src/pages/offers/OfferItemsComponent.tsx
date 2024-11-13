import React, { useEffect, useState } from 'react';
import { Button, ButtonGroup, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from '@mui/material';
import { Add, Delete, Edit } from '@mui/icons-material';
import { ToolbarItem } from '../../components/contentBlocks/DefaultContentBlock';
import { getOffer } from '../../services/offer.service';
import { OfferDTO } from '../../types/domain';
import { handleAsyncTask } from '../../utils/handleAsyncTask';
import { grey } from '@mui/material/colors';

interface OfferItemsComponentProps {
    offerId: string | undefined;
    setError: (error: any) => void;
    setToolbar?: (items: ToolbarItem[]) => void;
}

const OfferItemsComponent: React.FC<OfferItemsComponentProps> = ({ offerId, setError, setToolbar }) => {

    const [data, setData] = useState<OfferDTO>();
    const toolbarItemsItems: ToolbarItem[] = [
        {
            text: "Neu",
            route: `/offers/${offerId}/items/create`,
            icon: <Add />
        }
    ];

    useEffect(() => {
        loadData();
        setToolbar && setToolbar(toolbarItemsItems);
    }, [offerId]);

    const loadData = () => handleAsyncTask({
        task: () => getOffer(offerId!, true),
        onSuccess: setData,
        onError: setError
    });

    return (
        <TableContainer sx={{ p: 0 }}>
            <Table size='small' >
                <TableHead>
                    <TableRow>
                        <TableCell variant='head' style={{ width: 'auto' }}><b>Pos.</b></TableCell>
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
                                <TableCell sx={{ verticalAlign: 'top' }} rowSpan={2} align="right">
                                    <ButtonGroup variant="text" size='small' color='secondary'>
                                        <Button LinkComponent="a" href={`/offers/${data?.id?.value}/items/${item.id?.value}`}><Edit /></Button>
                                        <Button LinkComponent="a" color='error' href={`/offers/${data?.id?.value}/items/${item.id?.value}/delete`}><Delete /></Button>
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

export default OfferItemsComponent;