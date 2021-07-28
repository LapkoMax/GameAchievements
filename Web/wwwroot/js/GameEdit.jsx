class GenreList extends React.Component {
    constructor(props) {
        super(props);
        this.state = { genres: this.props.genres };
        this.onDeleteClick = this.onDeleteClick.bind(this);
    }
    onDeleteClick(e) {
        this.state.genres.splice(e.target.value, 1);
        this.setState({ genres: this.state.genres });
        this.props.updateGenres(this.state.genres);
    }
    render() {
        return (
            this.state.genres.map(genre => (
                <form name="genreList" key={genre.id}>
                    <label class="col-lg-2 col-md-3 col-sm-3 col-form-label text-center mt-1">{genre.name}</label>
                    <button class="btn btn-danger col-lg-2 col-md-2 col-sm-2 mt-4" type="button" value={this.state.genres.indexOf(genre)} onClick={this.onDeleteClick} >Delete</button><br />
                </form>
            )));
    }
}

class AddGenreList extends React.Component {
    constructor(props) {
        super(props);
        this.state = { genres: this.props.genres };
    }
    render() {
        return (
            this.state.genres.map(genre => (
                <option key={genre.id} value={genre.id}>
                    {genre.name}
                </option>
            )));
    }
}

class EditForm extends React.Component {
    constructor(props) {
        super(props);
        let genresNames = this.props.game.genres.split(', ');
        let genres = [];
        let isAdd = false;
        this.props.genresData.forEach(genreData => {
            genresNames.forEach(genreName => {
                if (genreName == genreData.name) {
                    isAdd = true;
                }
            });
            if (isAdd) {
                genres.push(genreData);
            }
            isAdd = false;
        });
        this.state = { name: this.props.game.name, description: this.props.game.description, rating: this.props.game.rating, genres: genres };
        this.handleNameChange = this.handleNameChange.bind(this);
        this.handleDescriptionChange = this.handleDescriptionChange.bind(this);
        this.handleRatingChange = this.handleRatingChange.bind(this);
        this.onCancelClick = this.onCancelClick.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.updateGenres = this.updateGenres.bind(this);
        this.onOptionClick = this.onOptionClick.bind(this);
    }
    handleNameChange(e) {
        this.setState({ name: e.target.value });
    }
    handleDescriptionChange(e) {
        this.setState({ description: e.target.value });
    }
    handleRatingChange(e) {
        this.setState({ rating: e.target.value });
    }
    onCancelClick(e) {
        window.location.href = this.props.url;
    }
    handleSubmit(e) {
        e.preventDefault();
        const name = this.state.name.trim();
        const description = this.state.description.trim();
        let ratingStr = this.state.rating + '';
        if (ratingStr.includes(".")) {
            let nums = ratingStr.trim().split(".");
            ratingStr = (nums[0] + "," + nums[1]);
        }
        const rating = ratingStr;

        const data = new FormData();
        data.append('Name', name);
        data.append('Description', description);
        data.append('Rating', rating);

        let genreIds = '';
        this.state.genres.forEach(genre => {
            genreIds += genre.id + ' ';
        });

        const xhr = new XMLHttpRequest();
        xhr.open('post', this.props.updateUrl + "?id=" + this.props.gameId, true);
        xhr.send(data);

        const newxhr = new XMLHttpRequest();
        newxhr.open('post', this.props.updateGenresUrl + "?genreIds=" + genreIds.trim() + "&gameId=" + this.props.gameId, true);
        newxhr.onload = () => this.onCancelClick();
        newxhr.send();
    }
    updateGenres(genres) {
        this.setState({ genres: genres });
        this.state.genres.forEach(genre => console.log(genre.name));
        console.log(" ");
    }
    onOptionClick(e) {
        if (e.target.value.includes("Choose")) return;
        let isAdd = true;
        this.props.genresData.forEach(genreData => {
            if (genreData.id == e.target.value) {
                this.state.genres.forEach(genre => {
                    if (genre.id == genreData.id) isAdd = false;
                });
                if (isAdd) this.state.genres.push(genreData);
                isAdd = true;
            }
        });
        this.updateGenres(this.state.genres);
    }
    render() {
        return (
            <form className="editForm" onSubmit={this.handleSubmit} >
                <label>Game Name:</label><br />
                <input
                    type="text"
                    value={this.state.name}
                    onChange={this.handleNameChange}
                /><br />
                <label>Game Description:</label><br />
                <textarea
                    rows="5"
                    cols="80"
                    type="text"
                    value={this.state.description}
                    onChange={this.handleDescriptionChange}
                /><br />
                <label>Game Rating:</label><br />
                <input
                    type="number"
                    step="0.1"
                    min="0"
                    max="10"
                    value={this.state.rating}
                    onChange={this.handleRatingChange}
                /><br />
                <label>Game Genres:</label><br />
                <GenreList genres={this.state.genres} updateGenres={this.updateGenres} /><br />
                <label>Add new genres:</label>
                <select id="Genres" onClick={this.onOptionClick} defaultValue="Choose genres">
                    <option key="0" disabled>Choose genres</option>
                    <AddGenreList genres={this.props.genresData} />
                </select><br /><br />
                <input type="submit" value="Save Game" />
                <button type="button" name="Cancel" onClick={this.onCancelClick} >Cancel</button>
            </form>
        );
    }
}