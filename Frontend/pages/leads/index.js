import React, {Component, Fragment} from 'react';
import {post} from '../../helpers/HttpClient';
import Table from '../../components/Table/Table';
import {showError} from '../../helpers/Notificator';
import PhoneLink from '../../components/PhoneLink/PhoneLink';
import MailLink from '../../components/MailLink/MailLink';
import styles from './index.less';
import AddressLink from "../../components/AddressLink/AddressLink";
import Currency from "../../components/Currency/Currency";
import Date from "../../components/Date/Date";
import Modal from "../../components/Modal/Modal";
import Button from "../../components/Button/Button";
import Dropdown from "../../components/Dropdown/Dropdown";

const urls = {
    getList: 'Api/V1/Leads/GetList',
    get: 'Api/V1/Leads/Get',
    create: 'Api/V1/Leads/Create',
    update: 'Api/V1/Leads/Update',
    delete: 'Api/V1/Leads/Delete',
    restore: 'Api/V1/Leads/Restore',
    leadSourcesGetFilterList: 'Api/V1/LeadSources/GetFilterList'
};

export class Leads extends Component {
    constructor() {
        super();

        this.state = {
            data: [],
            leadSources: [],
            leadSource: 0,
            modalData: {},
            isModalVisible: false
        }
    }

    componentDidMount() {
        document.title = 'Лиды';

        this.getLeadSourcesGetFilterList();
        this.getList();
    }

    render() {
        return (
            <div className={styles.layout}>
                <div className={styles.tableWrapper}>
                    {this.renderTable()}
                    {this.renderModal()}
                </div>
                <div className={styles.filterWrapper}>
                    {this.renderFilters()}
                </div>
            </div>
        );
    }

    renderTable() {
        return (
            <Table data={this.state.data}
                   onClickEdit={row => this.onClickRow(row)}
                   onClickDelete={row => this.onClickDelete(row)}
                   columns={[
                       {
                           title: 'ФИО',
                           width: 225,
                           formatter: (x, row) => `${row.surname} ${row.name} ${row.patronymic}`
                       },
                       {
                           title: 'Наименование организации',
                           column: 'companyName',
                           width: 300
                       },
                       {
                           title: 'Должность',
                           column: 'post',
                           width: 160
                       },
                       {
                           title: 'Телефон',
                           width: 105,
                           column: 'phone',
                           formatter: x => (<PhoneLink value={x}/>)
                       },
                       {
                           title: 'Email',
                           width: 160,
                           column: 'email',
                           formatter: x => (<MailLink value={x}/>)
                       },
                       {
                           title: 'Адрес',
                           width: 700,
                           formatter: (x, row) => this.renderAddressLink(row)
                       },
                       {
                           title: 'Вероятная сумма сделки',
                           column: 'opportunitySum',
                           formatter: x => (<Currency value={x}/>),
                           width: 110
                       },
                       {
                           title: 'Создан',
                           column: 'createDate',
                           formatter: x => (<Date value={x}/>),
                           width: 140
                       }
                   ]}>
            </Table>
        );
    }

    renderFilters() {
        const data = [{value: 0, label: 'Не выбрано'}, ... this.state.leadSources.map(x => {
            return {value: x.id, label: x.name}
        })];

        return (
            <div>
                <Dropdown value={this.state.leadSource} width={300} data={data}
                          onSelect={({value}) => this.setState({leadSource: value})}/>
            </div>
        );
    }

    renderModal() {
        if (!this.state.isModalVisible) {
            return null;
        }

        const row = this.state.modalData;

        return (
            <Modal className={styles.modal} visible={this.state.isModalVisible} width={800}
                   onClickClose={() => this.onClickCloseModal()}>
                <h2>{`${row.surname} ${row.name} ${row.patronymic}`}</h2>
                <p>Наименование организации: {row.companyName}</p>
                <p>Должность: {row.post}</p>
                <p>Телефон: <PhoneLink value={row.phone}/></p>
                <p>Email: <MailLink value={row.email}/></p>
                <p>Адрес: {this.renderAddressLink(row)}</p>
                <p>Вероятная сумма сделки: <Currency value={row.opportunitySum}/></p>
                <p>Создан: <Date value={row.createDate}/></p>

                <div className={styles.modalButtonsWrapper}>
                    <Button className={styles.modalButton} icon='remove'
                            onClick={() => alert('Удален')}>Удалить</Button>
                    <Button className={styles.modalButton} icon='edit'
                            onClick={() => alert('Изменение')}>Изменить</Button>
                    <Button className={styles.modalButton} icon='close'
                            onClick={() => this.onClickCloseModal()}>Закрыть</Button>
                </div>
            </Modal>
        );
    }

    renderAddressLink(row) {
        return (
            <AddressLink postcode={row.postcode} region={row.region} province={row.province}
                         street={row.street} city={row.city} street={row.street} house={row.house}
                         apartment={row.apartment}/>
        );
    }

    getList() {
        const data = {
            size: 10
        };

        post(urls.getList, data)
            .then(response => {

                this.setState({
                    data: response
                });
            })
            .catch(error => {
                showError(error);
            })
    }

    getLeadSourcesGetFilterList() {
        post(urls.leadSourcesGetFilterList)
            .then(response => {
                this.setState({
                    leadSources: response
                });
            })
            .catch(error => {
                showError(error);
            })
    }

    onClickRow(row) {
        this.setState({
            modalData: row,
            isModalVisible: true
        });
    }

    onClickCloseModal() {
        this.setState({isModalVisible: false})
    }

    onClickDelete(row) {

    }
}