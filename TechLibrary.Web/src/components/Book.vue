<template>
    <div v-if="!isEditMode">
        <b-card :title="title"
                :img-src="thumbnailUrl"
                img-alt="Image"
                img-top
                tag="article"
                style="max-width: 30rem;"
                class="mb-2">
            <b-card-text>
                {{ shortDesc }}
            </b-card-text>

            <b-button variant="secondary" to="/">Back</b-button>
            <b-button variant="primary" class="ml-1" @click='editMode()'>Edit</b-button>
        </b-card>
    </div>
    <div v-else>
        <b-form @submit="onSubmit">
            <b-form-group id="input-group-title"
                          label="Title:"
                          label-for="input-title">
                <b-form-input id="input-title"
                              v-model="title"
                              placeholder="Enter book's title"
                              required></b-form-input>
            </b-form-group>

            <b-form-group id="input-group-isbn"
                          label="ISBN:"
                          label-for="input-isbn">
                <b-form-input id="input-isbn"
                              v-model="isbn"
                              placeholder="Enter book's ISBN"
                              required></b-form-input>
            </b-form-group>

            <b-form-group id="input-group-publishedDate"
                          label="Published Date:"
                          label-for="input-publishedDate">
                <b-form-datepicker id="input-publishedDate"
                                   v-model="publishedDate"
                                   placeholder="Enter book's published date"></b-form-datepicker>
            </b-form-group>

            <b-form-group id="input-group-thumbnailUrl"
                          label="Thumbnail Url:"
                          label-for="input-thumbnailUrl">
                <b-form-input id="input-thumbnailUrl"
                              v-model="thumbnailUrl"
                              placeholder="Enter Thumbnail Url"></b-form-input>
            </b-form-group>

            <b-form-group id="input-group-shortDesc"
                          label="Short Description:"
                          label-for="input-shortDesc">
                <b-form-input id="input-shortDesc"
                              v-model="shortDesc"
                              placeholder="Enter short description of the book"></b-form-input>
            </b-form-group>

            <b-form-group id="input-group-longDesc"
                          label="Long Description:"
                          label-for="input-longDesc">
                <b-form-textarea id="input-longDesc"
                                 v-model="longDesc"
                                 placeholder="Enter long description of the book"
                                 rows="3"
                                 max-rows="6"></b-form-textarea>
            </b-form-group>

            <b-button variant="primary" type="submit">Submit</b-button>
            <b-button variant="danger" class="ml-1" type="reset">Reset</b-button>
            <b-button variant="secondary" class="ml-1" @click='editMode()'>Back</b-button>
        </b-form>
    </div>
</template>

<script>
    import axios from 'axios';

    export default {
        name: 'Book',
        props: ["id"],
        data: () => ({
            isEditMode: false,
            bookId: -1,
            title: null,
            isbn: null,
            publishedDate: null,
            thumbnailUrl: null,
            shortDesc: null,
            longDesc: null
        }),
        mounted() {
            if (this.id) {
                axios.get(`https://localhost:5001/books/${this.id}`)
                    .then(response => {
                        this.isEditMode = false;

                        this.bookId = response.data.bookId;
                        this.title = response.data.title;
                        this.isbn = response.data.isbn;
                        this.publishedDate = response.data.publishedDate;
                        this.thumbnailUrl = response.data.thumbnailUrl;
                        this.shortDesc = response.data.descr;
                    });
            }
            else {
                this.isEditMode = true;
            }
        },
        methods: {
            async onSubmit(event) {
                event.preventDefault();
                let request = {
                    "bookId": this.bookId,
                    "title": this.title,
                    "isbn": this.isbn,
                    "publishedDate": this.publishedDate,
                    "thumbnailUrl": this.thumbnailUrl,
                    "shortDescr": this.shortDesc,
                    "longDescr": this.longDesc
                }
                await axios.post('https://localhost:5001/books/Save', request)
                    .then(response => {
                        this.isEditMode = false;

                        this.bookId = response.data.bookId;
                        this.title = response.data.title;
                        this.isbn = response.data.isbn;
                        this.publishedDate = response.data.publishedDate;
                        this.thumbnailUrl = response.data.thumbnailUrl;
                        this.shortDesc = response.data.descr;
                        this.longDesc = null;
                    });
            },
            editMode: function () {
                this.isEditMode = !this.isEditMode;
            }
        }
    }
</script>