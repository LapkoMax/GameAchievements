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
                <form class="form" name="genreList">
                    <label class="col-lg-3 col-md-3 col-sm-3 col-form-label text-center mt-1">{genre.name}</label>
                    <button class="btn btn-danger col-lg-2 col-md-2 col-sm-2" type="button" value={this.state.genres.indexOf(genre)} onClick={this.onDeleteClick} >Delete</button>
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
                <option value={genre.id} title={genre.description}>
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
            <form class="form-control" className="editForm" onSubmit={this.handleSubmit} >
                <div class="col-10 row">
                    <label class="d-flex col-lg-2 col-md-3 col-sm-3 col-form-label text-center mt-1">Game Name:</label>
                    <input
                        type="text"
                        class="d-flex col-lg-3 col-md-3 col-sm-3 mt-1"
                        value={this.state.name}
                        onChange={this.handleNameChange}
                    />
                </div>
                <div class="col-6 row">
                    <label class="d-flex col-lg-3 col-md-3 col-sm-3 col-form-label text-center mt-1">Game Description:</label>
                    <div class="col-lg-9 col-md-9 col-sm-9">
                        <textarea
                            class="form-control"
                            rows="5"
                            type="text"
                            value={this.state.description}
                            onChange={this.handleDescriptionChange}
                        />
                    </div>
                </div>
                <div class="col-10 row">
                    <label class="d-flex col-lg-2 col-md-3 col-sm-3 col-form-label text-center mt-1">Game Rating:</label>
                    <input
                        type="number"
                        class="d-flex col-lg-3 col-md-3 col-sm-3 mt-1"
                        step="0.1"
                        min="0"
                        max="10"
                        value={this.state.rating}
                        onChange={this.handleRatingChange}
                    />
                </div>
                <label class="d-flex col-lg-2 col-md-3 col-sm-3 col-form-label text-center mt-1">Game Genres:</label>
                <div class="d-flex col-6 row">
                    <GenreList genres={this.state.genres} updateGenres={this.updateGenres} />
                </div>
                <label class="d-flex col-lg-2 col-md 3 col-sm-3">Add new genres:</label>
                <div class="col-lg-2 col-md-3 col-sm-3">
                    <select class="form-select" onClick={this.onOptionClick}>
                        <option disabled selected>Choose genres</option>
                        <AddGenreList genres={this.props.genresData} />
                    </select>
                </div>
                <button class="btn btn-primary col-lg-1 col-md-1 col-sm-1 mt-4" type="submit">Save game</button>
                <button class="btn btn-secondary col-lg-1 col-md-1 col-sm-1 mt-4" type="button" name="Cancel" onClick={this.onCancelClick} >Cancel</button>
            </form>
        );
    }
}