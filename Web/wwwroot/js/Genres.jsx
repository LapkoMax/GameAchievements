class FirstGenreRow extends React.Component {
    render() {
        return (
            <thead>
                <tr>
                    <th class="text-center" scope="col">Name</th>
                    <th class="text-center" scope="col">Description</th>
                </tr>
            </thead>
        );
    }
}

class GenreRows extends React.Component {
    render() {
        return this.props.data.map(genre => (
            <tr>
                <td class="text-center">{genre.name}</td>
                <td class="text-center">{genre.description}</td>
                <td><button class="btn btn-danger" value={genre.id} type="button" onClick={this.props.onDeleteClick}>Delete</button></td>
                <td><button class="btn btn-primary" value={genre.id} type="button" onClick={this.props.onEditClick}>Edit</button></td>
            </tr>
        ));

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

class GenreParametersForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = { sortBy: '', byDesc: '', searchBy: '' };
        this.onSortOptionClick = this.onSortOptionClick.bind(this);
        this.onSortDescClick = this.onSortDescClick.bind(this);
        this.handleSearchByChange = this.handleSearchByChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }
    onSortOptionClick(e) {
        this.setState({ sortBy: e.target.value });
    }
    onSortDescClick(e) {
        this.setState({ byDesc: e.target.checked });
    }
    handleSearchByChange(e) {
        this.setState({ searchBy: e.target.value });
    }
    handleSubmit(e) {
        e.preventDefault();
        let sortBy = this.state.sortBy.trim();
        if (this.state.byDesc == true) sortBy += " desc";
        const searchBy = this.state.searchBy;
        this.props.loadGenreOptions({ sortBy: sortBy, searchBy: searchBy });
    }
    render() {
        return (
            <form className="genreParametersForm" onSubmit={this.handleSubmit} >
                <div class="form-group row">
                    <label class="col-lg-1 col-md-2 col-sm-2 col-form-label text-center mt-1">Sort by:</label>
                    <div class="d-flex col-lg-3 col-md-3 col-sm-3">
                        <select class="form-select" onClick={this.onSortOptionClick}>
                            <option disabled selected>Choose field</option>
                            <option value="name">Name</option>
                            <option value="description">Description</option>
                        </select>
                    </div>
                    <input class="form-check-input mt-3" type="checkbox" value="" id="sortDesc" onClick={this.onSortDescClick} />
                    <label class="form-check-label col-lg-2 col-md-2 col-sm-2 col-form-label text-center mt-1" for="sortDesc">By descending</label>
                    <div class="col-12 row">
                        <label class="col-lg-2 col-md-3 col-sm-3 col-form-label text-center mt-1">Search by name:</label>
                        <input
                            type="text"
                            class="d-flex col-lg-3 col-md-3 col-sm-3 mt-1"
                            placeholder="Name"
                            value={this.state.searchBy}
                            onChange={this.handleSearchByChange}
                        />
                    </div>
                    <div class="col-12 row">
                        <label class="col-lg-1 col-md-1 col-sm-12 col-form-label">
                        </label>
                        <button class="btn btn-primary col-lg-2 col-md-2 col-sm-2 mt-4" type="submit">Accept</button>
                    </div>
                </div>
            </form>
        );
    }
}

class GenreTable extends React.Component {
    constructor(props) {
        super(props);
        this.state = { data: this.props.initialData, options: '' };
        this.handleGenreSubmit = this.handleGenreSubmit.bind(this);
        this.onGenreDelete = this.onGenreDelete.bind(this);
        this.onGenreEdit = this.onGenreEdit.bind(this);
        this.loadGenreOptions = this.loadGenreOptions.bind(this);
    }
    loadGenresFromServer() {
        const xhr = new XMLHttpRequest();
        xhr.open('get', this.props.url + this.state.options, true);
        xhr.onload = () => {
            const data = JSON.parse(xhr.responseText);
            this.setState({ data: data });
        };
        xhr.send();
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
        this.setState({ options: optionsStr });
    }
    componentDidMount() {
        window.setInterval(
            () => this.loadGenresFromServer(),
            this.props.pollInterval,
        );
    }
    render() {
        return (
            <div className="table">
                <GenreParametersForm loadGenreOptions={this.loadGenreOptions} />
                <table class="table table-bordered">
                    <FirstGenreRow />
                    <tbody>
                        <GenreRows data={this.state.data} onDeleteClick={this.onGenreDelete} onEditClick={this.onGenreEdit} />
                    </tbody>
                </table>
                <GenreForm onGenreSubmit={this.handleGenreSubmit} />
            </div>
        );
    }
}