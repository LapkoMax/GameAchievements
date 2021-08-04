class FirstGenreRow extends React.Component {
    render() {
        var components = [];
        if (this.props.fields.includes("name"))
            components.push(<th class="text-center" scope="col">Name</th>);
        if (this.props.fields.includes("description"))
            components.push(<th class="text-center" scope="col">Description</th>);
        return (
            <thead>
                <tr>
                    {components}
                </tr>
            </thead>
        );
    }
}

class GenreRows extends React.Component {
    render() {
        var components = [];
        var component = [];
        this.props.data.forEach(genre => {
            if (this.props.fields.includes("name"))
                component.push(<td class="text-center">{genre.name}</td>);
            if (this.props.fields.includes("description"))
                component.push(<td class="text-center">{genre.description}</td>);
            component.push(<td class="text-center"><button class="btn btn-danger" value={genre.id} type="button" onClick={this.props.onDeleteClick}>Delete</button></td>);
            component.push(<td class="text-center"><button class="btn btn-primary" value={genre.id} type="button" onClick={this.props.onEditClick}>Edit</button></td>);
            components.push(<tr>{component}</tr>);
            component = [];
        });
        return (
            <tbody>
                {components}
            </tbody>
        );

    }
}

class GenreForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = { name: '', description: '' };
        this.handleNameChange = this.handleNameChange.bind(this);
        this.handleDescriptionChange = this.handleDescriptionChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }
    handleNameChange(e) {
        this.setState({ name: e.target.value });
    }
    handleDescriptionChange(e) {
        this.setState({ description: e.target.value });
    }
    handleSubmit(e) {
        e.preventDefault();
        const name = this.state.name.trim();
        const description = this.state.description.trim();
        if (!name || !description) {
            return;
        }
        this.props.onGenreSubmit({ name: name, description: description });
        this.setState({ name: '', description: '' });
    }
    render() {
        return (
            <form class="form" className="genreForm" align="center" onSubmit={this.handleSubmit}>
                <h3 class="d-flex">Create new genre:</h3>
                <div class="col-lg-4 col-md-4 col-sm-4">
                    <input
                        type="text"
                        class="form-control"
                        placeholder="Genre name"
                        value={this.state.name}
                        onChange={this.handleNameChange}
                    />
                    <input
                        type="text"
                        class="form-control"
                        placeholder="Genre description"
                        value={this.state.description}
                        onChange={this.handleDescriptionChange}
                    />
                </div>
                <div class="d-flex col-12">
                    <input class="btn btn-outline-primary" type="submit" value="Create Genre" />
                </div>
            </form>
        );
    }
}

class GenrePager extends React.Component {
    constructor(props) {
        super(props);
        this.state = { currentPage: this.props.metaData.currentPage, page: '', pageSize: '' };
        this.handlePageChange = this.handlePageChange.bind(this);
        this.onGoClick = this.onGoClick.bind(this);
        this.onPageSizeChange = this.onPageSizeChange.bind(this);
        this.onChangeClick = this.onChangeClick.bind(this);
        this.handlePageClick = this.handlePageClick.bind(this);
    }
    handlePageChange(e) {
        this.setState({ page: e.target.value });
    }
    onGoClick() {
        var currentPage = this.state.page;
        this.props.loadGenrePageOptions({ currentPage: currentPage });
    }
    onPageSizeChange(e) {
        this.setState({ pageSize: e.target.value });
    }
    onChangeClick() {
        var newPageSize = this.state.pageSize;
        this.props.changePageSize({ pageSize: newPageSize });
    }
    handlePageClick(e) {
        var currentPage = this.state.currentPage;
        if (e.target.value == "Prev") {
            this.setState({ currentPage: +this.state.currentPage - 1 });
            currentPage -= 1;
        }
        else if (e.target.value == "Next") {
            this.setState({ currentPage: +this.state.currentPage + 1 });
            currentPage += 1;
        }
        else {
            this.setState({ currentPage: e.target.value });
            currentPage = e.target.value;
        }
        this.props.loadGenrePageOptions({ currentPage: currentPage });
    }
    render() {
        var pageNums = [];
        var components = [];
        for (let i = 1; i <= parseInt(this.props.metaData.totalPages); i++) {
            pageNums.push(i);
        }
        if (this.props.metaData.hasPrevious) components.push(<button class="btn btn-outline-primary col-lg-1 col-mg-1 col-sm-1" value="Prev" onClick={this.handlePageClick}>&laquo;</button>);
        else components.push(<button class="btn btn-outline-primary col-lg-1 col-mg-1 col-sm-1" value="Prev" onClick={this.handlePageClick} disabled>&laquo;</button>);
        pageNums.map(num => {
            if (num == 1 && num != this.props.metaData.currentPage) {
                components.push(<button class="btn btn-outline-primary col-lg-1 col-mg-1 col-sm-1" value={num} onClick={this.handlePageClick}>{num}</button>);
                if (this.props.metaData.currentPage > 3 && this.props.metaData.totalPages >= 5) components.push(<label class="text-center col-lg-1 col-mg-1 col-sm-1">...</label>);
            }
            else if (num == this.props.metaData.totalPages && num != this.props.metaData.currentPage) {
                if (this.props.metaData.currentPage < this.props.metaData.totalPages - 2 && this.props.metaData.totalPages >= 5) components.push(<label class="text-center col-lg-1 col-mg-1 col-sm-1">...</label>);
                components.push(<button class="btn btn-outline-primary col-lg-1 col-mg-1 col-sm-1" value={num} onClick={this.handlePageClick}>{num}</button>);
            }
            else if (num == this.props.metaData.currentPage)
                components.push(<button class="btn btn-primary col-lg-1 col-mg-1 col-sm-1" disabled>{num}</button>);
            else if (num == this.props.metaData.currentPage - 1 || num == this.props.metaData.currentPage + 1)
                components.push(<button class="btn btn-outline-primary col-lg-1 col-mg-1 col-sm-1" value={num} onClick={this.handlePageClick}>{num}</button>);
            else {
                if (this.props.metaData.totalPages < 5) components.push(<button class="btn btn-outline-primary col-lg-1 col-mg-1 col-sm-1" value={num} onClick={this.handlePageClick}>{num}</button>);
            }
        })
        if (this.props.metaData.hasNext) components.push(<button class="btn btn-outline-primary col-lg-1 col-mg-1 col-sm-1" value="Next" onClick={this.handlePageClick}>&raquo;</button>);
        else components.push(<button class="btn btn-outline-primary col-lg-1 col-mg-1 col-sm-1" value="Prev" onClick={this.handlePageClick} disabled>&raquo;</button>);
        return (
            <form class="text-center form col-12 row">
                <div class="col-12 row">
                    <div class="col-lg-5 col-md-5 col-sm-5">
                        {components.map(component => (
                            component
                        ))}
                    </div>
                    <div class="col-lg-1 col-md-2 col-sm-2 mt-1 row"><input
                        type="number"
                        step="1"
                        min="1"
                        max={this.props.metaData.totalPages}
                        class="form-control"
                        placeholder="Page"
                        value={this.state.page}
                        onChange={this.handlePageChange}
                    /></div>
                    <button class="btn btn-primary col-lg-1 col-mg-1 col-sm-1 mt-1" onClick={this.onGoClick}>Go</button>
                    <div class="col-lg-2 col-md-2 col-sm-2 mt-1 row">
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <select class="form-select" onClick={this.onPageSizeChange}>
                                <option disabled selected>Page size</option>
                                <option value="10">10</option>
                                <option value="25">25</option>
                                <option value="50">50</option>
                                <option value="100">100</option>
                                <option value="150">150</option>
                                <option value="200">200</option>
                                <option value={this.props.metaData.totalCount}>Total: {this.props.metaData.totalCount}</option>
                            </select>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3"><button class="btn btn-primary" onClick={this.onChangeClick}>Change</button></div>
                    </div>
                </div>
            </form>
        );
    }
}

class GenreParametersForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = { sortBy: '', byDesc: '', searchBy: '', fields: "name description" };
        this.onSortOptionClick = this.onSortOptionClick.bind(this);
        this.onSortDescClick = this.onSortDescClick.bind(this);
        this.onFieldCheckClick = this.onFieldCheckClick.bind(this);
        this.handleSearchByChange = this.handleSearchByChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }
    onSortOptionClick(e) {
        this.setState({ sortBy: e.target.value });
    }
    onSortDescClick(e) {
        this.setState({ byDesc: e.target.checked });
    }
    onFieldCheckClick(e) {
        var fields = this.state.fields;
        if (e.target.checked && !this.state.fields.includes(e.target.value)) fields += ' ' + e.target.value;
        else if (!e.target.checked && this.state.fields.includes(e.target.value)) {
            var fieldList = fields.split(' ');
            fields = '';
            fieldList.forEach(field => {
                if (field != e.target.value) fields += field + ' ';
            });
        }
        this.setState({ fields: fields.trim() });
    }
    handleSearchByChange(e) {
        this.setState({ searchBy: e.target.value });
    }
    handleSubmit(e) {
        e.preventDefault();
        let sortBy = this.state.sortBy.trim();
        if (this.state.byDesc == true) sortBy += " desc";
        const searchBy = this.state.searchBy;
        let fields = this.state.fields;
        this.props.loadGenreOptions({ sortBy: sortBy, searchBy: searchBy, fields: fields });
    }
    render() {
        return (
            <form className="genreParametersForm" onSubmit={this.handleSubmit} >
                <div class="form-group row">
                    <div class="col-5 row">
                        <div class="col-10 row">
                            <label class="d-flex col-lg-4 col-md-4 col-sm-4 mt-1 col-form-label text-center">Sort by:</label>
                            <div class="col-lg-4 col-md-4 col-sm-4 mt-1">
                                <select class="form-select text-truncate" onClick={this.onSortOptionClick}>
                                    <option disabled selected>Choose field</option>
                                    <option value="name">Name</option>
                                    <option value="description">Description</option>
                                </select>
                            </div>
                            <div class="form-check col-lg-4 col-md-4 col-sm-4">
                                <input class="form-check-input mt-3" type="checkbox" value="" id="sortDesc" onClick={this.onSortDescClick} />
                                <label class="form-check-label col-lg-12 col-md-12 col-sm-12 col-form-label text-left mt-1" for="sortDesc">By descending</label>
                            </div>
                        </div>
                        <div class="col-11 row">
                            <label class="d-flex col-lg-4 col-md-4 col-sm-4 col-form-label text-center mt-1">Search by name:</label>
                            <input
                                type="text"
                                class="col-lg-4 col-md-4 col-sm-4 mt-1"
                                placeholder="Name"
                                value={this.state.searchBy}
                                onChange={this.handleSearchByChange}
                            />
                        </div>
                        <div class="col-10 mt-3 row">
                            <button class="btn btn-primary col-lg-4 col-md-4 col-sm-4" type="submit">Accept</button>
                        </div>
                    </div>
                    <div class="col-6 row">
                        <label class="col-form-label col-lg-1 col-md-1 col-sm-1 mt-1">Fields:</label>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <input class="form-check-input mt-3" type="checkbox" value="name" id="nameField" onClick={this.onFieldCheckClick} defaultChecked />
                            <label class="form-check-label col-lg-9 col-md-9 col-sm-9 col-form-label text-center mt-1" for="nameField">Name</label>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <input class="form-check-input mt-3" type="checkbox" value="description" id="descriptionField" onClick={this.onFieldCheckClick} defaultChecked />
                            <label class="form-check-label col-lg-9 col-md-9 col-sm-9 col-form-label text-center mt-1" for="descriptionField">Description</label>
                        </div>
                    </div>
                    <GenrePager metaData={this.props.metaData} loadGenrePageOptions={this.props.loadGenrePageOptions} changePageSize={this.props.changePageSize} />
                </div>
            </form>
        );
    }
}

class GenreTable extends React.Component {
    constructor(props) {
        super(props);
        this.state = { data: this.props.initialData, metaData: this.props.metaData, options: '', pageNumber: this.props.metaData.pageNumber, pageSize: this.props.metaData.pageSize, fields: "name description" };
        this.handleGenreSubmit = this.handleGenreSubmit.bind(this);
        this.onGenreDelete = this.onGenreDelete.bind(this);
        this.onGenreEdit = this.onGenreEdit.bind(this);
        this.loadGenreOptions = this.loadGenreOptions.bind(this);
        this.loadGenrePageOptions = this.loadGenrePageOptions.bind(this);
        this.changePageSize = this.changePageSize.bind(this);
    }
    loadGenresFromServer() {
        const xhr = new XMLHttpRequest();
        xhr.open('get', this.props.url + this.state.options, true);
        xhr.onload = () => {
            const data = JSON.parse(xhr.responseText);
            this.setState({ data: data });
        };
        xhr.send();

        const newXhr = new XMLHttpRequest();
        newXhr.open('get', this.props.getMetaDataUrl + this.state.options, true);
        newXhr.onload = () => {
            const metaData = JSON.parse(newXhr.responseText);
            this.setState({ metaData: metaData });
        };
        newXhr.send();
    }
    onGenreDelete(e) {
        const xhr = new XMLHttpRequest();
        xhr.open('post', this.props.deleteUrl + "?genreId=" + e.target.value, true);
        xhr.onload = () => this.loadGenresFromServer();
        xhr.send();
    }
    onGenreEdit(e) {
        var url = this.props.editUrl + "?genreId=" + e.target.value;
        window.location.href = url;
    }
    handleGenreSubmit(genre) {
        const data = new FormData();
        data.append('Name', genre.name);
        data.append('Description', genre.description);

        const xhr = new XMLHttpRequest();
        xhr.open('post', this.props.creationUrl, true);
        xhr.onload = () => this.loadGamesFromServer();
        xhr.send(data);
    }
    loadGenreOptions(options) {
        let optionsStr = "";
        if (!options.sortBy) optionsStr = "?orderBy=name";
        else optionsStr = "?orderBy=" + options.sortBy;
        if (options.searchBy) optionsStr += "&searchTerm=" + options.searchBy;
        if (options.minRating) optionsStr += "&minRating=" + options.minRating;
        if (options.maxRating) optionsStr += "&maxRating=" + options.maxRating;
        optionsStr += "&pageNumber=" + this.state.pageNumber;
        optionsStr += "&pageSize=" + this.state.pageSize;
        let fields = options.fields;
        this.setState({ options: optionsStr, fields: fields });
    }
    loadGenrePageOptions(options) {
        if (options.currentPage) this.setState({ pageNumber: options.currentPage });
    }
    changePageSize(options) {
        if (options.pageSize) this.setState({ pageSize: options.pageSize });
    }
    componentDidMount() {
        window.setInterval(
            () => this.loadGenresFromServer(),
            this.props.pollInternal,
        );
    }
    render() {
        return (
            <div className="table">
                <GenreParametersForm loadGenreOptions={this.loadGenreOptions} loadGenrePageOptions={this.loadGenrePageOptions} changePageSize={this.changePageSize} metaData={this.state.metaData}/>
                <table class="table table-bordered">
                    <FirstGenreRow fields={this.state.fields} />
                    <GenreRows data={this.state.data} onDeleteClick={this.onGenreDelete} onEditClick={this.onGenreEdit} fields={this.state.fields} />
                </table>
                <GenreForm onGenreSubmit={this.handleGenreSubmit} />
            </div>
        );
    }
}