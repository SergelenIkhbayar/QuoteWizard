<template>
    <div class="home">
        <b-container>
            <b-row>
                <b-col>
                    <b-input-group size="sm">
                        <b-form-input id="search-input"
                                      v-model="searchValue"
                                      type="search"
                                      placeholder="Search books"></b-form-input>
                        <b-input-group-append>
                            <b-button :disabled="!searchValue" @click="searchValue = ''">Clear</b-button>
                        </b-input-group-append>
                    </b-input-group>
                </b-col>
                <b-col>
                    <b-form-radio-group v-model="isTitle"
                                        :options="searchOptions"
                                        value-field="value"
                                        text-field="desc">
                    </b-form-radio-group>
                </b-col>
                <b-col>
                    <b-button class="float-right" variant="primary" :to="{ name: 'book_view' }">Add new book</b-button>
                </b-col>
            </b-row>
        </b-container>

        <b-table striped hover :items="items" :fields="fields" responsive="sm" id="table" :current-page="pageNumber">
            <template v-slot:cell(thumbnailUrl)="data">
                <b-img :src="data.value" thumbnail fluid></b-img>
            </template>
            <template v-slot:cell(title_link)="data">
                <b-link :to="{ name: 'book_view', params: { 'id' : data.item.bookId } }">{{ data.item.title }}</b-link>
            </template>
        </b-table>
        <b-pagination v-model="pageNumber" :total-rows="pagination" :per-page="perPage"></b-pagination>

        <p class="mt-3">Current Page: {{ pageNumber }}</p>
    </div>
</template>

<script>
    import axios from 'axios';

    export default {
        name: 'Home',
        data: () => ({
            fields: [
                { key: 'thumbnailUrl', label: 'Book Image' },
                { key: 'title_link', label: 'Book Title', sortable: true, sortDirection: 'desc' },
                { key: 'isbn', label: 'ISBN', sortable: true, sortDirection: 'desc' },
                { key: 'descr', label: 'Description', sortable: true, sortDirection: 'desc' }

            ],
            items: [],
            pageNumber: 1,
            pagination: 0,
            perPage: 10,

            isTitle: true,
            searchOptions: [
                { value: true, desc: "Title" },
                { value: false, desc: "Description" }
            ],
            searchValue: null
        }),
        created: function () {
            this.fetchBooks();
        },
        methods: {
            async fetchBooks() {
                let request = {
                    "pageNumber": this.pageNumber,
                    "isTitle": this.isTitle,
                    "searchValue": this.searchValue
                }
                this.items = await axios.post('https://localhost:5001/books', request)
                    .then(response => {
                        this.pageNumber = response.data.pageNumber;
                        this.pagination = response.data.pagination;
                        return response.data.bookResponses;
                    })
                    .then(items => items)
            }
        },
        watch: {
            pageNumber: {
                handler: function () {
                    this.fetchBooks().catch(error => {
                        console.error(error)
                    })
                }
            },
            isTitle: {
                handler: function () {
                    this.fetchBooks().catch(error => {
                        console.error(error)
                    })
                }
            },
            searchValue: {
                handler: function () {
                    this.fetchBooks().catch(error => {
                        console.error(error)
                    })
                }
            }
        }
    };
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>