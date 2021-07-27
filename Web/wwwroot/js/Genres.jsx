class FirstGenreRow extends React.Component {
    render() {
        return (
            <thead key="0" align="center">
                <tr>
                    <td>Name</td>
                    <td>Description</td>
                </tr>
            </thead>
        );
    }
}

class GenreRows extends React.Component {
    render() {
        return this.props.data.map(genre => (
            <tbody key={genre.id} align="center">
                <tr>
                    <td>{genre.name}</td>
                    <td>{genre.description}</td>
                    <td><button value={genre.id} type="submit" onClick={this.props.onDeleteClick}>Delete</button></td>
                    <td><button value={genre.id} type="submit" onClick={this.props.onEditClick}>Edit</button></td>
                </tr>
            </tbody>
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
            <form className="genreForm" align="center" onSubmit={this.handleSubmit}>
                <input
                    type="text"
                    placeholder="Genre name"
                    value={this.state.name}
                    onChange={this.handleNameChange}
                />
                <input
                    type="text"
                    placeholder="Genre description"
                    value={this.state.description}
                    onChange={this.handleDescriptionChange}
                />
                <input type="submit" value="Create Genre" />
            </form>
        );
    }
}

class GenreTable extends React.Component {
    constructor(props) {
        super(props);
        this.state = { data: this.props.initialData };
        this.handleGenreSubmit = this.handleGenreSubmit.bind(this);
        this.onGenreDelete = this.onGenreDelete.bind(this);
        this.onGenreEdit = this.onGenreEdit.bind(this);
    }
    loadGenresFromServer() {
        const xhr = new XMLHttpRequest();
        xhr.open('get', this.props.url, true);
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
    componentDidMount() {
        window.setInterval(
            () => this.loadGenresFromServer(),
            this.props.pollInterval,
        );
    }
    render() {
        return (
            <div className="table">
                <table width="80%" border="1" align="center">
                    <FirstGenreRow />
                    <GenreRows data={this.state.data} onDeleteClick={this.onGenreDelete} onEditClick={this.onGenreEdit} />
                </table>
                <GenreForm onGenreSubmit={this.handleGenreSubmit} />
            </div>
        );
    }
}