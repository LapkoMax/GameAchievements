class FirstAchievementRow extends React.Component {
    render() {
        return (
            <thead key="0" align="center">
                <tr>
                    <th class="text-center" scope="col">Name</th>
                    <th class="text-center" scope="col">Description</th>
                    <th class="text-center" scope="col">Condition</th>
                </tr>
            </thead>
        );
    }
}

class AchievementRows extends React.Component {
    render() {
        return this.props.data.map(achievement => (
            <tr>
                <td class="text-center">{achievement.name}</td>
                <td class="text-center">{achievement.description}</td>
                <td class="text-center">{achievement.condition}</td>
                <td class="text-center"><button class="btn btn-danger" value={achievement.id} type="button" onClick={this.props.onDeleteClick}>Delete</button></td>
                <td class="text-center"><button class="btn btn-primary" value={achievement.id} type="button" onClick={this.props.onEditClick}>Edit</button></td>
            </tr>
        ));

    }
}

class AchievementForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = { name: '', description: '', condition: '' };
        this.handleNameChange = this.handleNameChange.bind(this);
        this.handleDescriptionChange = this.handleDescriptionChange.bind(this);
        this.handleConditionChange = this.handleConditionChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }
    handleNameChange(e) {
        this.setState({ name: e.target.value });
    }
    handleDescriptionChange(e) {
        this.setState({ description: e.target.value });
    }
    handleConditionChange(e) {
        this.setState({ condition: e.target.value });
    }
    handleSubmit(e) {
        e.preventDefault();
        const name = this.state.name.trim();
        const description = this.state.description.trim();
        const condition = this.state.condition.trim();
        if (!name || !description || !condition) {
            return;
        }
        this.props.onGenreSubmit({ name: name, description: description, condition: condition });
        this.setState({ name: '', description: '', condition: '' });
    }
    render() {
        return (
            <form class="form" className="achievementForm" align="center" onSubmit={this.handleSubmit}>
                <h3 class="d-flex">Create new achievement:</h3>
                <div class="form-group row col-lg-4 col-md-4 col-sm-4">
                    <input
                        type="text"
                        class="form-control"
                        placeholder="Achievement name"
                        value={this.state.name}
                        onChange={this.handleNameChange}
                    />
                    <input
                        type="text"
                        class="form-control"
                        placeholder="Achievement description"
                        value={this.state.description}
                        onChange={this.handleDescriptionChange}
                    />
                    <input
                        type="text"
                        class="form-control"
                        placeholder="Achievement condition"
                        value={this.state.condition}
                        onChange={this.handleConditionChange}
                    />
                </div>
                <div class="d-flex row col-lg-2 col-md-3 col-sm-3">
                    <input class="btn btn-outline-primary" type="submit" value="Create Achievement" />
                </div>
            </form>
        );
    }
}

class AchievementPager extends React.Component {
    constructor(props) {
        super(props);
        this.state = { currentPage: this.props.metaData.currentPage, page: '' };
        this.handlePageChange = this.handlePageChange.bind(this);
        this.onGoClick = this.onGoClick.bind(this);
        this.handlePageClick = this.handlePageClick.bind(this);
    }
    handlePageChange(e) {
        this.setState({ page: e.target.value });
    }
    onGoClick() {
        var currentPage = this.state.page;
        this.props.loadAchievementPageOptions({ currentPage: currentPage });
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
        this.props.loadAchievementPageOptions({ currentPage: currentPage });
    }
    render() {
        var pageNums = [];
        var components = [];
        for (let i = 1; i <= parseInt(this.props.metaData.totalPages); i++) {
            pageNums.push(i);
        }
        if (this.props.metaData.hasPrevious) components.push(<button class="btn btn-outline-primary col-lg-1 col-mg-1 col-sm-1" value="Prev" onClick={this.handlePageClick}>Prev</button>);
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
        if (this.props.metaData.hasNext) components.push(<button class="btn btn-outline-primary col-lg-1 col-mg-1 col-sm-1" value="Next" onClick={this.handlePageClick}>Next</button>);
        return (
            <form class="text-center form-control col-12 row">
                <div class="col-12 row">
                    <div class="col-lg-9 col-md-9 col-sm-9">
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
                </div>
            </form>
        );
    }
}

class AchievementParametersForm extends React.Component {
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
        this.props.loadAchievementOptions({ sortBy: sortBy, searchBy: searchBy });
    }
    render() {
        return (
            <form className="achievementParametersForm" onSubmit={this.handleSubmit} >
                <div class="form-group row">
                    <label class="col-lg-1 col-md-2 col-sm-2 col-form-label text-center mt-1">Sort by:</label>
                    <div class="d-flex col-lg-3 col-md-3 col-sm-3">
                        <select class="form-select" onClick={this.onSortOptionClick}>
                            <option disabled selected>Choose field</option>
                            <option value="name">Name</option>
                            <option value="description">Description</option>
                            <option value="condition">Condition</option>
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
                    <AchievementPager metaData={this.props.metaData} loadAchievementPageOptions={this.props.loadAchievementPageOptions} />
                </div>
            </form>
        );
    }
}

class AchievementsTable extends React.Component {
    constructor(props) {
        super(props);
        this.state = { data: this.props.initialData, options: '', metaData: this.props.metaData, pageNumber: this.props.metaData.pageNumber };
        this.handleAchievementSubmit = this.handleAchievementSubmit.bind(this);
        this.onAchievementDelete = this.onAchievementDelete.bind(this);
        this.onAchievementEdit = this.onAchievementEdit.bind(this);
        this.loadAchievementOptions = this.loadAchievementOptions.bind(this);
        this.loadAchievementPageOptions = this.loadAchievementPageOptions.bind(this);
        this.onCancelClick = this.onCancelClick.bind(this);
    }
    loadAchievementsFromServer() {
        const xhr = new XMLHttpRequest();
        xhr.open('get', this.props.url + "?gameId=" + this.props.gameId + this.state.options, true);
        xhr.onload = () => {
            const data = JSON.parse(xhr.responseText);
            this.setState({ data: data });
        };
        xhr.send();

        const newXhr = new XMLHttpRequest();
        newXhr.open('get', this.props.getMetaDataUrl + "?gameId=" + this.props.gameId + this.state.options, true);
        newXhr.onload = () => {
            const metaData = JSON.parse(newXhr.responseText);
            this.setState({ metaData: metaData });
        };
        newXhr.send();
    }
    onAchievementDelete(e) {
        const xhr = new XMLHttpRequest();
        xhr.open('post', this.props.deleteUrl + "?gameId=" + this.props.gameId + "&achievementId=" + e.target.value, true);
        xhr.onload = () => this.loadAchievementsFromServer();
        xhr.send();
    }
    onAchievementEdit(e) {
        var url = this.props.editUrl + "?gameId=" + this.props.gameId + "&id=" + e.target.value;
        window.location.href = url;
    }
    onCancelClick() {
        window.location.href = this.props.cancelUrl;
    }
    handleAchievementSubmit(achievement) {
        const data = new FormData();
        data.append('Name', achievement.name);
        data.append('Description', achievement.description);
        data.append('Condition', achievement.condition);

        const xhr = new XMLHttpRequest();
        xhr.open('post', this.props.creationUrl + "?gameId=" + this.props.gameId, true);
        xhr.onload = () => this.loadGamesFromServer();
        xhr.send(data);
    }
    loadAchievementOptions(options) {
        let optionsStr = "";
        if (!options.sortBy) optionsStr = "&orderBy=name";
        else optionsStr = "&orderBy=" + options.sortBy;
        if (options.searchBy) optionsStr += "&searchTerm=" + options.searchBy;
        if (options.minRating) optionsStr += "&minRating=" + options.minRating;
        if (options.maxRating) optionsStr += "&maxRating=" + options.maxRating;
        optionsStr += "&pageNumber=" + this.state.pageNumber;
        this.setState({ options: optionsStr });
    }
    loadAchievementPageOptions(options) {
        if (options.currentPage) this.setState({ pageNumber: options.currentPage });
    }
    componentDidMount() {
        window.setInterval(
            () => this.loadAchievementsFromServer(),
            this.props.pollInternal,
        );
    }
    render() {
        return (
            <div className="table">
                <AchievementParametersForm loadAchievementOptions={this.loadAchievementOptions} loadAchievementPageOptions={this.loadAchievementPageOptions} metaData={this.state.metaData}/>
                <table class="table table-bordered">
                    <FirstAchievementRow />
                    <tbody>
                        <AchievementRows data={this.state.data} onDeleteClick={this.onAchievementDelete} onEditClick={this.onAchievementEdit} />
                    </tbody>
                </table>
                <AchievementForm onGenreSubmit={this.handleAchievementSubmit} />
                <div class="col-12 row">
                    <button class="btn btn-secondary col-lg-2 col-md-2 col-sm-2 mt-4" type="button" onClick={this.onCancelClick} >Cancel</button>
                </div>
            </div>
        );
    }
}